
using ApiService;
using Domain.DTO;
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

    private static void Main(string[] args)
    {
        
        var builder = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        InputDirectory = config["AppSettings:InPutDirectory"];
        OutputDirectory = config["AppSettings:OutPutDirectory"];
        TreatedDirectory = config["AppSettings:TreatedDirectory"];
        RejectedDirectory = config["AppSettings:RejectedDirectory"];
        LogDirectory = config["AppSettings:LogDirectory"];
        APIUrl = config["AppSettings:API"];

        // Modifier la configuration après
        var logConfig = LogManager.Configuration;
        var fileTarget = logConfig.AllTargets.FirstOrDefault(t => t is FileTarget) as FileTarget;
        if (fileTarget != null)
        {
            fileTarget.FileName = LogDirectory + "/${date:format=yyyy-MM-dd}.log";
            LogManager.Configuration = logConfig;
            LogManager.ReconfigExistingLoggers();
        }


        log.Info($"chemin d'input : {InputDirectory}");
        log.Info($"chemin de sortie : {OutputDirectory}");
        log.Info($"chemin de fichier JMX traité : {TreatedDirectory}");
        log.Info($"chemin de fichier JMX rejté : {RejectedDirectory}");
        log.Info($"chemin de fichier Logs : {LogDirectory}");
        if (!string.IsNullOrWhiteSpace(InputDirectory) && !string.IsNullOrWhiteSpace(OutputDirectory))
        {
            log.Info("Start the application");
            DoWork();
            Console.ReadLine();
        }
       
    }

    public static async void DoWork()
    {
        while (true)
        {
            log.Info("lencement de recherche ");

            var files = Directory.GetFiles(InputDirectory, "*.jmx").Take(4).ToList();
            if (files != null && files.Any())
            {
                log.Info("des fichier sont détecter : " + string.Join(", ", files));
                foreach (var file in files)
                {
                    //Task.Run(() => ProcessFile(file)).ContinueWith(task =>
                    //{
                    //    task.Dispose();
                    //});
                    await ProcessFile(file);
                }
            }
            else
            {
                log.Info("pas de fichier jmx dans le dossier input");
            }

            Thread.Sleep(1000);
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
                    log.Warn($"le fichier  : {Path.GetFileName(file)} est déjà traiter !!");
                    log.Info($"déplacement de fichier : {Path.GetFileName(file)} dans le répertoire Rejet");
                    //Task.Run(() => Move(file, true)).ContinueWith(task =>
                    //{
                    //    task.Dispose();
                    //});
                    await Move(file, true);
                }
                else
                {
                    log.Info("debut de traitement pour le fichier  : " + Path.GetFileName(file));
                    try
                    {
                        var jmx = await JmxProcessor.DoWork(new string[] { file }, OutputDirectory);
                        log.Info("Image et fichier XML sont exporter avec succée pour le fichier : " + Path.GetFileName(file));
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
                        log.Info("Historique envoyer pour le fichier  : " + Path.GetFileName(file));
                        log.Info($"déplacement de fichier : {Path.GetFileName(file)} dans le répertoire Traité");
                        //Task.Run(() => Move(file, false)).ContinueWith(task =>
                        //{
                        //    task.Dispose();
                        //});
                        await Move(file, false);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.ToString());
                    }

                }
            }
            else
            {
                log.Info("pas de reponse depuis le serveur ");
            }

        }
        catch (Exception ex)
        {
            log.Error(ex.ToString());
        }
        
    }


    public static async Task Move(string file, bool isFailure)
    {
        if (isFailure)
        {
            if (!Directory.Exists(RejectedDirectory))
            {
                Directory.CreateDirectory(RejectedDirectory);
            }
            System.IO.File.Move(file, Path.Combine(RejectedDirectory, Path.GetFileName(file)));
        }
        else
        {
            if (!Directory.Exists(TreatedDirectory))
            {
                Directory.CreateDirectory(TreatedDirectory);
            }
            System.IO.File.Move(file, Path.Combine(TreatedDirectory, Path.GetFileName(file)));
        }
    }
}