using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Almostengr.FalconPiMonitor.ConsoleCmd
{
    public class InstallSystemdConsoleCmd : BaseConsoleCmd, IConsoleCmd
    {
        public void Run()
        {
            try
            {
                ConsoleMessage("Installing service");

                ConsoleMessage("Copying service file");
                File.Copy(string.Concat(AppDirectory, "/", ServiceFilename),
                    string.Concat(SystemdDirectory, "/", ServiceFilename));

                IList commands = new List<string>();
                commands.Add("sudo /bin/systemctl status falconpimonitor");
                commands.Add("sudo /bin/systemctl daemon-reload");
                commands.Add("sudo /bin/systemctl enable falconpimonitor");
                commands.Add("sudo /bin/systemctl start falconpimonitor");
                commands.Add("sudo /bin/systemctl status falconpimonitor");

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

                ConsoleMessage("Done installing service");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}