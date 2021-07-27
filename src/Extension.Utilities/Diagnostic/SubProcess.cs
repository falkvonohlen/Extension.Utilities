using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Utilities.Diagnostic
{
    public class SubProcess
    {
        private Action<string> _standardOut;
        private Action<string> _errorOut;

        public SubProcess(Action<string> standardOut, Action<string> errorOut)
        {
            _standardOut = standardOut;
            _errorOut = errorOut;
        }

        /// <summary>
        /// Execute a single command
        /// </summary>
        /// <param name="workingDir"></param>
        /// <param name="command"></param>
        public async Task<int> ExecuteAsync(string workingDir, string command, string arguments)
        {
            Process process = null;
            try
            {
                var psi = GetCommonPSI();
                psi.FileName = command;
                psi.Arguments = arguments;
                psi.WorkingDirectory = workingDir;
                process = Process.Start(psi);
                await WaitForExit(process);
                return process.ExitCode;
            }
            finally
            {
                KillProcess(process);
            }
        }

        /// <summary>
        /// Start output and error callback task and a wait for exit task.
        /// If all of thoses complete the process is closed
        /// </summary>
        private Task WaitForExit(Process process)
        {
            return Task.WhenAll(WaitForExitAsync(process), StartoutputCallbackTask(process), StarterrorCallbackTask(process));
        }

        /// <summary>
        /// Waituntil the process has exited
        /// </summary>
        /// <returns></returns>
        private Task WaitForExitAsync(Process process)
        {
            return Task.Run(() =>
            {
                process.WaitForExit();
            });
        }

        /// <summary>
        /// Start to redirect the output messages
        /// </summary>
        /// <returns></returns>
        private Task StartoutputCallbackTask(Process process)
        {
            return Task.Run(() =>
            {
                try
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        switch (line)
                        {
                            case string s when (s.Contains("echo") || string.IsNullOrEmpty(s)):
                                //We do not handle echo commands 
                                break;
                            default:
                                _standardOut(line);
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    _errorOut($"Process({process.Id}): output redirect failed");
                }

                _standardOut($"Process({process.Id}): Output stream closed");
            });
        }

        /// <summary>
        /// Start to redirect the error messages
        /// </summary>
        /// <returns></returns>
        private Task StarterrorCallbackTask(Process process)
        {
            return Task.Run(() =>
            {
                try
                {
                    while (!process.StandardError.EndOfStream)
                    {
                        var line = process.StandardError.ReadLine();
                        _errorOut(line);
                    }
                }
                catch (Exception)
                {
                    _errorOut($"Process({process.Id}): error redirect failed");
                }

                _standardOut($"Process({process.Id}): Error stream closed");
            });
        }

        /// <summary>
        /// Returns the commonly used psi
        /// </summary>
        /// <returns></returns>
        private ProcessStartInfo GetCommonPSI()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;
            return psi;
        }

        /// <summary>
        /// Kills the process in case of an excpetion
        /// </summary>
        private void KillProcess(Process process)
        {
            if (process != null)
            {
                process.Kill();
                process.Dispose();
            }
        }

    }
}
