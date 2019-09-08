using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace GuiTools
{
    public partial class MessagesForm : Form
    {
        public bool IgnoreResetStatus { get; set; }

        private RichTextBox m_contextMenuRtb;

        private DateTime m_lastStatusUpdate = DateTime.Now;
        
        public MessagesForm()
        {
            InitializeComponent();

            rtbConsoleOutput.MouseUp += rtb_MouseUp;
            rtbDebugOutput.MouseUp += rtb_MouseUp;
            rtbErrorOutput.MouseUp += rtb_MouseUp;
        }

        private void MessagesForm_Load(object sender, EventArgs e)
        {
            G.COutput += G_COutput;
            G.DOutput += G_DOutput;
            G.ErrorOutput += G_ErrorOutput;

        }

        // TODO: Limit size of output lists so as to not overrun memory

        private void G_COutput(MessageArgs e)
        {
            AppendText(rtbConsoleOutput, e.Text);
        }

        private void G_DOutput(MessageArgs e)
        {
            AppendText(rtbDebugOutput, e.Text);
        }

        private void G_ErrorOutput(MessageArgs e)
        {
            AppendText(rtbErrorOutput, e.Text);
        }

        private void AppendText(RichTextBox rtb, string text)
        {
            if (rtb.InvokeRequired)
            {
                this.Invoke(new Action<RichTextBox, string>(AppendText), rtb, text);
            }
            else
            {
                const int MAX_TEXT_LENGTH = 65536;
                if (rtb.TextLength > MAX_TEXT_LENGTH)
                {
                    rtb.Clear();
                }
                rtb.AppendText(text + "\n");
            }
        }

        private void MessagesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        /// <summary>
        /// For efficiency, we limit the list items to the last 500 displayed.
        /// </summary>
        /*private void LimitListItems()
        {
            try
            {
                // check if we need a delegate for this call
                if (lstData.InvokeRequired)
                {
                    this.Invoke(new UpdateControlsHandler(LimitListItems));
                }
                else
                {
                    while (lstData.Items.Count > 500)
                    {
                        lstData.Items.RemoveAt(500);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // the listbox went away.  Ingore it since we are probably exiting
            }
        }*/

        public void Status(string text)
        {
            try
            {
                if (statusStrip1.InvokeRequired) { statusStrip1.Invoke(new Action<string>(Status), text); }     // check if we need to invoke the delegate
                else
                {
                    // <GUI operations here>
                    tslblStatusLeft.Text = text;
                    m_lastStatusUpdate = DateTime.Now;
                    Application.DoEvents();
                }
            }
            catch (ObjectDisposedException) { }     // The GUI object went away, ignore it since we're probably exiting.
        }

        public void StatusAppend(string text)
        {
            Status(tslblStatusLeft.Text + text);
        }

        public void StatusColor(Color? color = null)
        {
            try
            {
                if (statusStrip1.InvokeRequired) { statusStrip1.Invoke(new Action<Color?>(StatusColor), color); }     // check if we need to invoke the delegate
                else
                {
                    // <GUI operations here>
                    tslblStatusLeft.BackColor = color ?? SystemColors.Control;
                    Application.DoEvents();
                }
            }
            catch (ObjectDisposedException) { }     // The GUI object went away, ignore it since we're probably exiting.
        }

        public void Status2(string text)
        {
            tslblStatusRight.Text = text;
            Application.DoEvents();
        }

        public void EnableButton(Button btn, bool b)
        {
            try
            {
                if (btn.InvokeRequired) { btn.Invoke(new Action<Button, bool>(EnableButton), btn, b); }     // check if we need to invoke the delegate
                else
                {
                    // <GUI operations here>
                    btn.Enabled = b;
                }
            }
            catch (ObjectDisposedException) { }     // The GUI object went away, ignore it since we're probably exiting.
        }

        public void ExecutePython(string script_filename, string args)
        {
            //string script_filename = "playground_bitcoin.py";
            //string args = string.Format("KRAKEN_OHLC {0} {1}", cboKrakenSymbols.Text, cboKrakenMinutes.Text);

            //EnableButton(false);
            IgnoreResetStatus = true;
            Status("Running Python script: " + script_filename + " ...");
            StatusColor(Color.Yellow);

            GSystem.ProcessExited += GSystem_ProcessExited;

            StartStatusTimer();

            try
            {
                GSystem.RunPython(script_filename, args, this.Handle);
            }
            catch (Exception ex)
            {
                string errorMsg = string.Format("An error occurred attempting to launch Python.\n\nCheck the app settings to ensure your Python is configured correctly.\n({0})", ex.Message);
                MessageBox.Show(this, errorMsg, "Python Error");
            }
        }

        public void GSystem_ProcessExited(object sender, EventArgs e)
        {
            //EnableButton(true);
            StopStatusTimer();
            StatusAppend(" Done.");
            StatusColor();
            IgnoreResetStatus = false;
            GSystem.ProcessExited -= GSystem_ProcessExited;
        }

        public void StartStatusTimer()
        {
            //timerStatus.Enabled = true;
            //timerStatus.Start();
            Application.DoEvents();
        }

        public void StopStatusTimer()
        {
            //timerStatus.Stop();
            //timerStatus.Enabled = false;
            Application.DoEvents();
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine("timer");
            //Application.DoEvents();
        }

        #region ---------- CUT/COPY/PASTE -------------------------------------------------------------------------------------------------
        private void rtb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {   //click event
                //MessageBox.Show("you got it!");
                ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
                MenuItem menuItem = new MenuItem("Cut");
                menuItem.Click += new EventHandler(CutAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Copy");
                menuItem.Click += new EventHandler(CopyAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Paste");
                menuItem.Click += new EventHandler(PasteAction);
                contextMenu.MenuItems.Add(menuItem);

                m_contextMenuRtb = (sender as RichTextBox);
                m_contextMenuRtb.ContextMenu = contextMenu;
            }
        }

        void CutAction(object sender, EventArgs e)
        {
            //var rtb = sender as RichTextBox;
            m_contextMenuRtb.Cut();
        }

        /*void CopyAction(object sender, EventArgs e)
        {
            //Graphics objGraphics;
            Clipboard.SetData(DataFormats.Text, m_contextMenuRtb.SelectedText);  //   m_contextMenuRtb.SelectedRtf);
            Clipboard.Clear();
        }

        void PasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                m_contextMenuRtb.SelectedText = Clipboard.GetData(DataFormats.Text).ToString();
            }
        }*/

        void CopyAction(object sender, EventArgs e)
        {
            //var rtb = sender as RichTextBox;
            Clipboard.SetText(m_contextMenuRtb.SelectedText);
        }

        void PasteAction(object sender, EventArgs e)
        {
            //var rtb = sender as RichTextBox;
            if (Clipboard.ContainsText())
            {
                m_contextMenuRtb.Text += Clipboard.GetText(TextDataFormat.Text).ToString();
            }
        }
        #endregion ------------------------------------------------------------------------------------------------------------------------


    } // end of class

} // end of namespace
