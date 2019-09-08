using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
//using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Drawing;
using static Tools.G;
using Microsoft.Win32;
using System.Reflection;

namespace Tools
{
    public static class GSystem
    {
        static public event EventHandler ProcessExited;

        // TODO: Create a way to allow the user to change the Python exe
        private static string m_python27ExePathname;
        public static string Python27ExePathname
        {
            get
            {
                m_python27ExePathname = Path.Combine(GSystem.GetEnvironmentVariable("PYTHON27PATH") ?? @"C:\Python27", "python.exe");
                return m_python27ExePathname;
            }
            set
            {
                m_python27ExePathname = value;
                //GSystem.SetEnvironmentVariable("PYTHON27PATH", m_python27ExePathname);
            }
        }

        // TODO: Use this function to (at least) display the %DROPBOXPATH% environment variable (and any others this app cares about)
        // Get the value of a Windows environment variable
        // For example to get the value of the %WINDIR% environment variable, call this function with "windir" as name argument
        public static string GetEnvironmentVariable(string name)
        {
            string value = Environment.GetEnvironmentVariable(name);
            dout("Environment Variable: '{0}' = '{1}'", name, value);
            return value;
        }

        // Get the values of ALL Windows environment variables
        public static IDictionary<string, string> GetEnvironmentVariables()
        {
            SortedDictionary<string, string> result = new SortedDictionary<string, string>();
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
            {
                dout("Environment Var:  {0} = {1}", de.Key, de.Value);
                result[de.Key.ToString()] = de.Value.ToString();
            }
            return result;
        }

        // Set the value of a Windows environment variable
        public static void SetEnvironmentVariable(string name, string value)
        {
            Environment.SetEnvironmentVariable(name, value);
            dout("Environment Variable: '{0}' = '{1}'", name, value);
        }

        // Delete a Windows environment variable
        public static void DeleteEnvironmentVariable(string name)
        {
            Environment.SetEnvironmentVariable(name, null);            
            if (Environment.GetEnvironmentVariable(name) == null)                   // confirm the deletion
                dout("Environment variable '{0}' has been deleted.", name);
        }



        // Return the name of the startup executable's assembly
        public static string GetAssemblyName()
        {
            Assembly exeAssembly = Assembly.GetEntryAssembly();
            return exeAssembly.GetName().Name;
        }

        public static string BashExecute(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            //var escapedArgs = cmd;

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        /// <summary>
        /// Execute a shell command
        /// </summary>
        /// <param name="_FileToExecute">File/Command to execute</param>
        /// <param name="_CommandLine">Command line parameters to pass</param> 
        /// <param name="_outputMessage">returned string value after executing shell command</param> 
        /// <param name="_errorMessage">Error messages generated during shell execution</param> 
        static public void ExecuteShellCommand(string _FileToExecute, string _CommandLine, ref string _outputMessage, ref string _errorMessage)
        {
            // Set process variable
            // Provides access to local and remote processes and enables you to start and stop local system processes.
            System.Diagnostics.Process _Process = null;
            try
            {
                _Process = new System.Diagnostics.Process();

                // invokes the cmd process specifying the command to be executed.
                string _CMDProcess = string.Format(System.Globalization.CultureInfo.InvariantCulture, @"{0}\cmd.exe", new object[] { Environment.SystemDirectory });

                // pass executing file to cmd (Windows command interpreter) as a arguments
                // /C tells cmd that we want it to execute the command that follows, and then exit.
                string _Arguments = string.Format(System.Globalization.CultureInfo.InvariantCulture, "/C {0}", new object[] { _FileToExecute });

                // pass any command line parameters for execution
                if (_CommandLine != null && _CommandLine.Length > 0)
                {
                    _Arguments += string.Format(System.Globalization.CultureInfo.InvariantCulture, " {0}", new object[] { _CommandLine, System.Globalization.CultureInfo.InvariantCulture });
                }

                // Specifies a set of values used when starting a process.
                System.Diagnostics.ProcessStartInfo _ProcessStartInfo = new System.Diagnostics.ProcessStartInfo(_CMDProcess, _Arguments);
                // sets a value indicating not to start the process in a new window. 
                _ProcessStartInfo.CreateNoWindow = true;
                // sets a value indicating not to use the operating system shell to start the process. 
                _ProcessStartInfo.UseShellExecute = false;
                // sets a value that indicates the output/input/error of an application is written to the Process.
                _ProcessStartInfo.RedirectStandardOutput = true;
                _ProcessStartInfo.RedirectStandardInput = true;
                _ProcessStartInfo.RedirectStandardError = true;
                _Process.StartInfo = _ProcessStartInfo;

                // Starts a process resource and associates it with a Process component.
                _Process.Start();

                // Instructs the Process component to wait indefinitely for the associated process to exit.
                _errorMessage = _Process.StandardError.ReadToEnd();
                _Process.WaitForExit();

                // Instructs the Process component to wait indefinitely for the associated process to exit.
                _outputMessage = _Process.StandardOutput.ReadToEnd();
                _Process.WaitForExit();
            }
            catch (Win32Exception _Win32Exception)
            {
                // Error
                Console.WriteLine("Win32 Exception caught in process: {0}", _Win32Exception.ToString());
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }
            finally
            {
                // close process and do cleanup
                _Process.Close();
                _Process.Dispose();
                _Process = null;
            }
        }

        public static void ProcessStart(string executableFilename, string arguments, string workingDirectory = null, IntPtr? parentWindowHandle = null)
        {
            Process process = new Process();
            process.StartInfo.FileName = executableFilename;
            process.StartInfo.Arguments = arguments;
            if (workingDirectory != null)
                process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.ErrorDialog = true;
            if (parentWindowHandle != null)
                process.StartInfo.ErrorDialogParentHandle = parentWindowHandle.Value;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            //process.StartInfo.EnvironmentVariables;
            //process.StartInfo.CreateNoWindow = false;
            process.Start();
        }

        public static void ProcessStartSimple(string exeFilename, string arguments)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = exeFilename;
                process.StartInfo.Arguments = arguments;
                process.Start();
            }
            catch (Exception ex)
            {
                ErrorMessage("GSystem::ProcessStartSimple => {0}", ex.Message);
            }
        }

        //public static System.Collections.Generic.IEnumerable<string> RunPython(string pyscript, string pyargs = "", IntPtr? parentWindowHandle = null)
        public static void RunPython(string pyscript, string pyargs = "", IntPtr? parentWindowHandle = null)
        {
            /*string args = string.Format("{0} {1}", pyscript, pyargs);
            string workingDirectory = Folders.DropboxFolders.python_folder;

            ProcessStart(GSystem.Python27ExePathname, args, workingDirectory, parentWindowHandle);
            return;*/

            //rtbOutput.Clear();
            Directory.SetCurrentDirectory(Folders.DropboxFolders.python_folder);

            string argText = string.Format("{0} {1}", pyscript, pyargs);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            //p.FileName = GSystem.Python27ExePathname;
            startInfo.FileName = "python";
            startInfo.Arguments = argText;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;     // read forums that said you MUST also redirect standard input
            //p.Verb = "runas";
            startInfo.CreateNoWindow = true;
            
            dout("Launching python process: 'python {0}'", argText);

            Process p = new Process();
            p.StartInfo = startInfo;
            p.EnableRaisingEvents = true;
            p.OutputDataReceived += (s, e) => cout(e.Data);      //Console.WriteLine("received output: {0}", args.Data);
            p.ErrorDataReceived += (s, e) => ErrorMessage(e.Data);
            p.Exited += P_Exited;
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            
            //p.WaitForExit();

            /*using (Process process = Process.Start(startInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    //string result = reader.ReadToEnd();     // for async: string result = await reader.ReadToEndAsync();
                    //cout(result);
                    string result;
                    while (process.StandardOutput.Peek() > -1)
                    {
                        //string result = reader.ReadLine();
                        //coutFire(process.StandardOutput.ReadLine());
                        result = reader.ReadLine();
                        coutFire(result);
                        //result = reader.ReadLine();
                        //yield return result;
                    }
                    while (process.StandardError.Peek() > -1)
                    {
                        result = process.StandardError.ReadLine();
                        errorMessageFire(result);
                    }
                }
                process.WaitForExit();
            }*/

            //string strCmdText = "/C python " + script;
            //Process.Start("CMD.exe", strCmdText);
        }

        private static void P_Exited(object sender, EventArgs e)
        {
            ProcessExited?.Invoke(sender, e);
        }

        public static void PingTest(string url)
        {
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                // Use the default Ttl value which is 128, but change the fragmentation behavior.
                options.DontFragment = true;
                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(url, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    cout("Address: {0}", reply.Address.ToString());
                    cout("RoundTrip time: {0}", reply.RoundtripTime);
                    if (reply.Options != null)
                    {
                        cout("Time to live: {0}", reply.Options.Ttl);
                        cout("Don't fragment: {0}", reply.Options.DontFragment);
                    }
                    cout("Buffer size: {0}", reply.Buffer.Length);
                }
                else
                {
                    ErrorMessage("Ping test failed!");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage("Ping test failed: {0} '{1}'", url, ex.Message);
            }
        }

        public static void ShowThreadInfo(String name)
        {
            Console.WriteLine("{0} Thread ID: {1}", name, Thread.CurrentThread.ManagedThreadId);
        }

    } // end of class

} // end of namespace
