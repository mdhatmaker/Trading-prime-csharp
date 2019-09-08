using System;// I'm pretty sure this is required...
using System.Collections.Generic;// I don't think i used any collections
using System.Linq;// probably not needed at all.
using System.Windows.Forms;//Console app, but I like to show message boxes for errors...
using System.IO;// this is needed.
using System.Threading;// yay threads!
using System.Diagnostics;//Helpful in getting this code working.
using System.Xml;//not needed unless you want to try parsing the get*() functions.
using System.Xml.XPath;
using SharpVoice;
using SharpVoice.Util;
using System.Web;
using System.Net;

namespace GoogleTests
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try {

                Visual Form = new Visual();
                Form.ShowDialog();
		    } catch (IOException e) {
                Debug.WriteLine(e.StackTrace);
                MessageBox.Show(e.Message);//more Debug help
		    }
        }
    }
}
