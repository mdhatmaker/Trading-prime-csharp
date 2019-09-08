using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;

namespace CryptoTools.Messaging
{
    public class EmailPub
    {
		private SmtpClient m_smtp;
		private string m_fromEmail;
		private MailAddress m_fromAddress;

		private bool m_mailSent = false;

        /*
            m_smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };
        */

        // where smtpHost like "smtp.gmail.com"
        // where fromAddress like "mhatmaker@gmail.com"
		public EmailPub(SmtpClient smtpClient, string fromEmail, string fromDisplayName="BOT-MSG")
        {
			m_smtp = smtpClient;
			m_fromEmail = fromEmail;

			// Specify the e-mail sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            //string address = "jane@contoso.com";
            //string displayName = "Jane " + (char)0xD8 + " Clayton";
            m_fromAddress = new MailAddress(fromEmail, fromDisplayName, System.Text.Encoding.UTF8);
        }

		/*// where toEmail like "luckyperson@online.microsoft.com"
		// where subject like "This is the Subject line"
		// where msg like "This is the message body"
        public void Send(string toEmail, string subject, string msg)
		{
			MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(toEmail);
            message.Subject = subject;
            message.From = new MailAddress(m_fromEmail);
            message.Body = msg;
			m_smtp.Send(message);
		}*/

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            m_mailSent = true;
        }
        
		// where toEmail like ben@contoso.com"
        // where subject like "This is the Subject line"
        // where msg like "This is the message body"
        public void SendAsync(string toEmail, string subject, string msg, string[] attachmentFilenames = null)
		{
			m_mailSent = false;
            // Set destinations for the e-mail message.
			MailAddress toAddress = new MailAddress(toEmail);
            // Specify the message content.
            MailMessage message = new MailMessage(m_fromAddress, toAddress);
            message.Body = msg;            
            //string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });  // Include some non-ASCII characters in body and subject.
            //message.Body += Environment.NewLine + someArrows;
            message.BodyEncoding = System.Text.Encoding.UTF8;
			message.Subject = subject;  // "test message 1" + someArrows;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Check for attachemnts
            if (attachmentFilenames != null)
            {
                foreach (var af in attachmentFilenames)
                {
                    message.Attachments.Add(new Attachment(af));
                }
            }
            // Set the method that is called back when the send operation ends.
            m_smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            // The userState can be any object that allows your callback 
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            string userState = "test_message_1";
            m_smtp.SendAsync(message, userState);
            //Console.WriteLine("Sending message...");
            //message.Dispose();                              // clean up
		}
        
        public void CancelSendAsync()
		{
			// If mail hasn't been sent yet, then cancel the pending operation
			if (m_mailSent == false)
            {
                m_smtp.SendAsyncCancel();
            }
		}

        // where toEmail like ben@contoso.com"
        // where subject like "This is the Subject line"
        // where msg like "This is the message body"
        public void Send(string toEmail, string subject, string msg, string[] attachmentFilenames = null)
        {
            // Set destinations for the e-mail message.
            MailAddress toAddress = new MailAddress(toEmail);
            // Specify the message content.
            MailMessage message = new MailMessage(m_fromAddress, toAddress);
            message.Body = msg;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = subject;  // "test message 1" + someArrows;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Check for attachemnts
            if (attachmentFilenames != null)
            {
                foreach (var af in attachmentFilenames)
                {
                    message.Attachments.Add(new Attachment(af));
                }
            }
            m_smtp.Send(message);
            //Console.WriteLine("Message sent.");
            message.Dispose();                              // clean up
        }

    } // end of class EmailPub

} // end of namespace
