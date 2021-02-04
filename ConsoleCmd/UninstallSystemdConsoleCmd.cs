using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Almostengr.FalconPiMonitor.ConsoleCmd
{
    public class UninstallSystemdConsoleCmd : BaseConsoleCmd
    {
        public void Run()
        {
            try
            {
                ConsoleMessage("Uninstalling service");

                IList commands = new List<string>();
                commands.Add("sudo /bin/systemctl status falconpimonitor");
                commands.Add("sudo /bin/systemctl stop falconpimonitor");
                commands.Add("sudo /bin/systemctl disable falconpimonitor");
                commands.Add("sudo /bin/systemctl status falconpimonitor");
                commands.Add("sudo /bin/systemctl daemon-reload");

                foreach (var command in commands)
                {
                    Process process = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            FileName = "/bin/bash",
                            Arguments = string.Concat("-c \"", command, "\""),
                            RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        }
                    };

                    ConsoleMessage($"{command}");
                    process.Start();

                    string result = process.StandardOutput.ReadToEnd();

                    process.WaitForExit();
                    Console.WriteLine(result);
                }

                ConsoleMessage("Removing service file");
                File.Delete(string.Concat(SystemdDirectory, "/", ServiceFilename));

                ConsoleMessage("Done uninstalling service");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}