
using ApiService;
using Domain.DTO;
using Domain.Models;
using FileProcessor;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Targets;
using System.Diagnostics;

internal class Program
{
    public static string InputDirectory;
   
    public static string OutputDirectory;
    public static string TreatedDirectory;
    public static string RejectedDirectory;
    public static string LogDirectory;
    public static string APIUrl;
    public static ILogger log = LogManager.Setup()
        .LoadConfigurationFromFile("nlog.config")
        .GetCurrentClassLogger();
    public static string _derectoryByDate
    {
        get
        {
            var dateNow = DateTime.Now;
            return Path.Combine(dateNow.Year.ToString(), dateNow.Month.ToString("00"), dateNow.Date.Day.ToString("00"));
        }
    }

    private static void Main(string[] args)
    {
        
        var builder = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();
        APIUrl = config["AppSettings:API"];
        Task.WaitAll(LoadConfig(config));

        // Modifier la configuration après
        var logConfig = LogManager.Configuration;
        var fileTarget = logConfig.AllTargets.FirstOrDefault(t => t is FileTarget) as FileTarget;
        if (fileTarget != null)
        {
            fileTarget.FileName = Path.Combine(LogDirectory ,_derectoryByDate) + "/${date:format=yyyy-MM-dd}.log";
            LogManager.Configuration = logConfig;
            LogManager.ReconfigExistingLoggers();
        }
        log.Info($"[PARAM] | INPUT PATH: {InputDirectory}");
        log.Info($"[PARAM] | OUTPUT PATH: {OutputDirectory}");
        log.Info($"[PARAM] | TREATED PATH: {TreatedDirectory}");
        log.Info($"[PARAM] | REJECTED PATH: {RejectedDirectory}");
        log.Info($"[PARAM] | LOG PATH: {LogDirectory}");


        if (!string.IsNullOrWhiteSpace(InputDirectory) && !string.IsNullOrWhiteSpace(OutputDirectory))
        {
            log.Info("[RUN] | Application starts...");
            DoWork();
            Console.ReadLine();
        }
    }

    public static async void DoWork()
    {
        while (true)
        {
            log.Info("[SEARCH] | Scan startup");
            var files = Directory.GetFiles(InputDirectory, "*.jmx").Take(4).ToList();
            if (files != null && files.Any())
            {
                log.Info("[SEARCH] | File(s) detected: " + string.Join(", ", files));
                foreach (var file in files)
                {
                    
                    await ProcessFile(file);
                }
            }
            else{
                log.Info("[SEARCH] | Any File was detected");
            }
            Thread.Sleep(3000);
        }
    }

    public static async Task ProcessFile(string file)
    {
        
        IProcess<List<string>> _processCheckFiles = new("data", APIUrl);
        try
        {
            var response = await _processCheckFiles.ProcessAsync<List<FileCheckDTO>>(new List<string>() { Path.GetFileName(file) }, RequestType.Post, EndPoint.checkfiles);
            if(response != null)
            {
                if (!response.FirstOrDefault().CanTreat)
                {
                    log.Warn($"[PROCESS] | The file {Path.GetFileName(file)} is already  treated.");
                    log.Info($"[PROCESS] | The file {Path.GetFileName(file)} was moved to 'REJECTED FOLDER'");
                    string device = JmxProcessor.GetDevice(file);
                    await Move(file, true, device);
                }
                else
                {
                    log.Info("[PROCESS] | Start moving the file: " + Path.GetFileName(file));
                    try
                    {
                        string _outPutDirectory = Path.Combine(OutputDirectory, _derectoryByDate);
                        var jmx = await JmxProcessor.DoWork(new string[] { file }, _outPutDirectory);
                        log.Info($"[PROCESS] | Images and XML file for {Path.GetFileName(file)} has been exported successfully.");
                        var lot = new Lot
                        {
                            Reference = "",
                            Documents = jmx
                                .Where(x => x?.DeploymentSummary != null && x?.VehicleDatas != null)
                                .Select(x => new JMX
                                {
                                    DeploymentSummary = x.DeploymentSummary,
                                    VehicleDatas = x.VehicleDatas
                                })
                                .Where(x => x.VehicleDatas != null && x.VehicleDatas.Any())
                                .Distinct()
                                .Select(jmx => new Document
                                {
                                    Jmx = jmx,
                                    Name = Path.GetFileName(file)
                                }).ToList()
                        };
                        IProcess<Lot> _process = new("data", APIUrl);
                        
                        await _process.ProcessAsync(lot, RequestType.Post, EndPoint.Add, false);
                        log.Info($"[API] | The data of {Path.GetFileName(file)} was sent to database.");
                        log.Info($"[PROCESS] | The file {Path.GetFileName(file)} was moved to 'TREATED FOLDER'");
                        //Task.Run(() => Move(file, false)).ContinueWith(task =>
                        //{
                        //    task.Dispose();
                        //});
                        await Move(file, false, jmx.FirstOrDefault()?.DeploymentSummary.CameraName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                    }
                }
            }
            else
            {
                log.Error("[API] | The API server is unreachable.");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.ToString());
        }
    }


    public static async Task Move(string file, bool isFailure, string cameraName = "")
    {
        if (isFailure)
        {
            string _rejected = Path.Combine(RejectedDirectory, _derectoryByDate, cameraName);
            if (!Directory.Exists(_rejected))
            {
                Directory.CreateDirectory(_rejected);
            }
            System.IO.File.Move(file, Path.Combine(_rejected, Path.GetFileName(file)));
        }
        else
        {
            string treated = Path.Combine(TreatedDirectory, _derectoryByDate, cameraName);
            if (!Directory.Exists(treated))
            {
                Directory.CreateDirectory(treated);
            }
            System.IO.File.Move(file, Path.Combine(treated, Path.GetFileName(file)));
        }
    }

    public static async Task LoadConfig(IConfiguration config)
    {
        IProcess<SettingDTO> _process = new("setting", APIUrl);
        
        var data = await _process.ProcessAsync(null, RequestType.Get, EndPoint.List);
        if (data != null && (data.Pagable?.Content?.Any() ?? false))
        {
            var input = data.Pagable?.Content.FirstOrDefault(x => x.Name == nameof(InputDirectory));
            if (input != null)
            {
                InputDirectory = input.Value;
            }

            var output = data.Pagable?.Content.FirstOrDefault(x => x.Name == nameof(OutputDirectory));
            if (output != null)
            {
                OutputDirectory = output.Value;
            }

            var treated = data.Pagable?.Content.FirstOrDefault(x => x.Name == nameof(TreatedDirectory));
            if (treated != null)
            {
                TreatedDirectory = treated.Value;
            }

            var reject = data.Pagable?.Content.FirstOrDefault(x => x.Name == nameof(RejectedDirectory));
            if (reject != null)
            {
                RejectedDirectory = reject.Value;
            }

            var log = data.Pagable?.Content.FirstOrDefault(x => x.Name == nameof(LogDirectory));
            if (log != null)
            {
                LogDirectory = log.Value;
            }
        }
        else
        {
            InputDirectory = config["AppSettings:InPutDirectory"];
            OutputDirectory = config["AppSettings:OutPutDirectory"];
            TreatedDirectory = config["AppSettings:TreatedDirectory"];
            RejectedDirectory = config["AppSettings:RejectedDirectory"];
            LogDirectory = config["AppSettings:LogDirectory"];
        }
    }
}
