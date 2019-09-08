using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpVoice;

namespace GoogleTests
{
    public partial class Visual : Form
    {
        Voice voiceConnection;

        public Visual()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                voiceConnection = new Voice(loginEmail.Text, loginPass.Text);
                callGroup.Enabled = true;
                smsGroup.Enabled = true;

                Folder inbox = voiceConnection.SMS;
                listView1.Groups.Add(Folder.SMS, "Inbox");
                foreach (SharpVoice.Message msg in inbox.Messages)
                {
                    ListViewItem item = new ListViewItem(listView1.Groups[Folder.SMS]);
                    item.Text = msg.DisplayNumber;
                    listView1.Items.Add(item);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void callStart_Click(object sender, EventArgs e)
        {
            string response = voiceConnection.Call(callTo.Text, callFrom.Text);
            MessageBox.Show(response);
        }

        private void countChars(object sender, EventArgs e)
        {
            smsChars.Text = smsMsg.TextLength + "/" + Math.Ceiling(smsMsg.TextLength / 160.0) * 160;
        }

        private void smsSend_Click(object sender, EventArgs e)
        {
            string response = voiceConnection.SendSMS(smsTo.Text, smsMsg.Text);
            MessageBox.Show(response);
        }
    }
}
