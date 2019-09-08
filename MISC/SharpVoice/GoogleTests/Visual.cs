using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            voiceConnection = new Voice(loginEmail.Text, loginPass.Text);
            voiceConnection.login();
            callGroup.Enabled = true;
            smsGroup.Enabled = true;
        }

        private void callStart_Click(object sender, EventArgs e)
        {
            string response = voiceConnection.call(callTo.Text, callFrom.Text);
            MessageBox.Show(response);
        }

        private void countChars(object sender, EventArgs e)
        {
            smsChars.Text = smsMsg.TextLength + "/" + Math.Ceiling(smsMsg.TextLength / 160.0) * 160;
        }

        private void smsSend_Click(object sender, EventArgs e)
        {
            string response = voiceConnection.sendSMS(smsTo.Text, smsMsg.Text);
            MessageBox.Show(response);
        }
    }
}
