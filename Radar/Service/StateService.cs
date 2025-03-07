using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Radar.Service
{
    public class ServerCredentials
    {
        public string ServerName { get; set; }
        public string Username { get; set; }
        public SecureString Password { get; set; }
        public string Port { get; set; }
        public bool UseSSL { get; set; }
        public bool isLocalHost { get; set; }
    }

    public class AppPoolStatus
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime LastChecked { get; set; }
    }

    public interface IStateService
    {
        Task<string> CheckIISStatusAsync(ServerCredentials credentials);
        Task<string> CheckServiceAsync(ServerCredentials credentials, string appPoolName);
        Task RestartIISAsync(ServerCredentials credentials);
        Task<AppPoolStatus> CheckAppPoolStatusAsync(ServerCredentials credentials, string appPoolName);
        Task RestartAppPoolAsync(ServerCredentials credentials, string appPoolName);
        Task RestartServiceAsync(ServerCredentials credentials, string appPoolName);
    }

    public class StateService : IStateService
    {
        public StateService()
        {
        }

        public async Task<string> CheckIISStatusAsync(ServerCredentials credentials)
        {
            const string script = @"
                try {
                    $service = Get-Service -Name W3SVC
                    $service.Status.ToString()
                } catch {
                    Write-Error $_.Exception.Message
                    throw
                }";
            return await ExecuteRemotePowerShellCommandAsync(credentials, script);
        }

        public async Task RestartIISAsync(ServerCredentials credentials)
        {
            const string script = @"
                try {
                    Restart-Service -Name W3SVC -Force
                    'IIS Service restarted successfully'
                } catch {
                    Write-Error $_.Exception.Message
                    throw
                }";
            await ExecuteRemotePowerShellCommandAsync(credentials, script);
        }

        public async Task<AppPoolStatus> CheckAppPoolStatusAsync(ServerCredentials credentials, string appPoolName)
        {
            string script = @"
                try {
                    $service = Get-IISAppPool -Name ""$($args[0])""
                    $service.Status.ToString()
                } catch {
                    Write-Error $_.Exception.Message
                    throw
                }";


            string jsonResult = await ExecuteRemotePowerShellCommandAsync(credentials, script, appPoolName);

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<AppPoolStatus>(jsonResult, options);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Failed to parse app pool status result: {jsonResult}", ex);
            }
        }

        public async Task RestartAppPoolAsync(ServerCredentials credentials, string appPoolName)
        {
            string script = @"
                try {
                    Import-Module WebAdministration -ErrorAction Stop
                    $pool = Get-Item ""IIS:\AppPools\$($args[0])"" -ErrorAction Stop
                    
                    if ($pool) {
                        Restart-WebAppPool -Name $($args[0])
                        'App Pool restarted successfully'
                    } else {
                        throw 'App Pool not found'
                    }
                } catch {
                    Write-Error ""App Pool Restart Error: $_""
                    throw
                }";

            await ExecuteRemotePowerShellCommandAsync(credentials, script, appPoolName);
        }

        public async Task<string> CheckServiceAsync(ServerCredentials credentials, string appPoolName)
        {
            string script = @"
                try {
                    $service = Get-Service -Name ""$($args[0])""
                    $service.Status.ToString()
                } catch {
                    Write-Error $_.Exception.Message
                    throw
                }";

            return await ExecuteRemotePowerShellCommandAsync(credentials, script, appPoolName);
        }

        public async Task RestartServiceAsync(ServerCredentials credentials, string appPoolName)
        {
            string script = @"Restart-Service -Name ""$($args[0])"" -Force";

            await ExecuteRemotePowerShellCommandAsync(credentials, script, appPoolName);
        }

        private async Task<string> ExecuteRemotePowerShellCommandAsync(ServerCredentials credentials, string script, params object[] parameters)
        {
            try
            {
                if (credentials.isLocalHost)
                {
                    // Create InitialSessionState without using statement
                    var initialSessionState = InitialSessionState.CreateDefault();
                    initialSessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;

                    using (var runspace = RunspaceFactory.CreateRunspace(initialSessionState))
                    {
                        runspace.Open();
                        Runspace.DefaultRunspace = runspace;
                        using (var powerShell = PowerShell.Create())
                        {
                            powerShell.Runspace = runspace;
                            powerShell.AddScript(script);

                            if (parameters != null && parameters.Length > 0)
                            {
                                powerShell.AddParameters(parameters);
                            }

                            var results = await powerShell.InvokeAsync();

                            if (powerShell.HadErrors)
                            {
                                var errors = powerShell.Streams.Error.Select(e => e.Exception?.Message ?? e.ToString());
                                throw new Exception($"PowerShell Errors: {string.Join(Environment.NewLine, errors)}");
                            }


                            return string.Join(Environment.NewLine, results.Select(x => x.ToString()));
                            
                        }
                    }
                }
                else
                {
                    var connectionInfo = new WSManConnectionInfo(
                        credentials.UseSSL ?
                            new Uri($"https://{credentials.ServerName}:{credentials.Port}/wsman") :
                            new Uri($"http://{credentials.ServerName}:{credentials.Port}/wsman"),
                        "http://schemas.microsoft.com/powershell/Microsoft.PowerShell",
                        new PSCredential(credentials.Username, credentials.Password))
                    {
                        AuthenticationMechanism = AuthenticationMechanism.Negotiate,
                        SkipCACheck = true,
                        SkipCNCheck = true,
                        SkipRevocationCheck = true
                    };

                    using (var runspace = RunspaceFactory.CreateRunspace(connectionInfo))
                    {
                        runspace.Open();
                        using (var powerShell = PowerShell.Create())
                        {
                            powerShell.Runspace = runspace;
                            powerShell.AddScript(script);

                            if (parameters != null && parameters.Length > 0)
                            {
                                powerShell.AddParameters(parameters);
                            }

                            var results = await powerShell.InvokeAsync();

                            if (powerShell.HadErrors)
                            {
                                var errors = powerShell.Streams.Error.Select(e => e.Exception?.Message ?? e.ToString());
                                throw new Exception($"PowerShell Errors: {string.Join(Environment.NewLine, errors)}");
                            }

                            return string.Join(Environment.NewLine, results.Select(x => x.ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to execute PowerShell command: {ex.Message}", ex);
            }
        }
    }
}
