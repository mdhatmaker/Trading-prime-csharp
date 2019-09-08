using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Tools;
//using GuiTools.Grid;
//using static Tools.GSystem;

namespace PrimeTrader
{
    public partial class BrowserForm : Form
    {
        Folders folders = Folders.DropboxFolders;

        public WebBrowser WebBrowser { get { return web; } }

        public BrowserForm()
        {
            InitializeComponent();
        }

        private void BrowserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        #region StatusBar --------------------------------------------------------------------------------------------------------------------------------------
        public void StatusLeft(string text)
        {
            tslblLeft.Text = text;
        }

        public void StatusMiddle(string text)
        {
            tslblMiddle.Text = text;
        }

        public void StatusRight(string text)
        {
            tslblRight.Text = text;
        }
        #endregion ---------------------------------------------------------------------------------------------------------------------------------------------


        public void Navigate(string url)
        {
            web.Navigate(url);
        }

        /*public async void Navigate(string url)
        {
            await Task.Run(() => web.Navigate(url));
            //web.Navigate(url);
        }*/

    } // end of class
} // end of namespace
