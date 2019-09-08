using System; 
using System.IO; 
using System.Diagnostics; 
 
namespace CallPython 
{ 
    /// <summary> 
    /// Used to show simple C# and Python interprocess communication 
    /// </summary> 
    class Program 
    { 
        static void Main(string[] args) 
        { 
            // full path of python interpreter  
            //string python = @"C:\Continuum\Anaconda\python.exe";
            string python = @"/Library/Frameworks/Python.framework/Versions/2.7/bin";
                    
            // python app to call  
            string myPythonApp = "sum.py";  
            
            // dummy parameters to send Python script  
            int x = 2;  
            int y = 5; 

            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python); 
            
            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false; 
            myProcessStartInfo.RedirectStandardOutput = true; 

            // start python app with 3 arguments  
            // 1st argument is pointer to itself, 2nd and 3rd are actual arguments we want to send 
            myProcessStartInfo.Arguments = myPythonApp + " " + x + " " + y;

            Process myProcess = new Process(); 
            // assign start information to the process 
            myProcess.StartInfo = myProcessStartInfo; 
            
            // start process 
            myProcess.Start();


            // We will perform synchronous read operations on the output stream of the
            // process using ReadLine(). However, in order to avoid deadlock we will
            // read output first and then wait for process terminate to close it.
            // NOTE: If we need to read multiple lines, we might use ReadToEnd() instead
            // of ReadLine().

            // Read the standard output of the app we called.  
            StreamReader myStreamReader = myProcess.StandardOutput; 
            string myString = myStreamReader.ReadLine(); 
            //string myString = myStreamReader.ReadToEnd();

            // wait exit signal from the app we called 
            myProcess.WaitForExit(); 
            
            // close the process 
            myProcess.Close();

            // write the output we got from python app 
            Console.WriteLine("Value received from script: " + myString);
        }
    }
}


