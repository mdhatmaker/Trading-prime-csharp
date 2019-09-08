using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CryptoTools.Messaging
{
    // Gmail publisher that allows for simple send email via Gmail account

    public class GmailPub
    {
        private SmtpClient m_smtp;
        private EmailPub m_epub;

        public GmailPub(string gmailAddress, string gmailPassword, string fromDisplayName)
        {
            m_smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(gmailAddress, gmailPassword)
            };

            m_epub = new EmailPub(m_smtp, gmailAddress, fromDisplayName);
        }

        public void Send(string toEmail, string subject, string msg, string[] attachmentFilenames = null)
        {
            m_epub.Send(toEmail, subject, msg, attachmentFilenames);
        }

    } // end of class GmailPub
} // end of namespace
