using System;
//using Twilio;
//using Twilio.Rest.Api.V2010.Account;
//using Twilio.Types;
using System.Net.Mail;
using System.Threading;

namespace CryptoTools.Messaging
{
	public enum CellularProvider { Alltel, ATT, Tmobile, VirginMobile, Sprint, Verizon, Nextel, USCellular };
   
	public class SmsPub
    {
		private EmailPub m_email;
        
        public SmsPub(SmtpClient smtpClient, string fromEmail, string displayName = "MSG-BOT")
		{
			m_email = new EmailPub(smtpClient, fromEmail, displayName);
		}

		public static void TestSms(string gmailFromAddress, string gmailPassword)
        {
			var fromEmail = gmailFromAddress;
			var fromPassword = gmailPassword;
            //var fromEmail = m_api.Credentials["GMAIL"].Key;           //var fromEmail = "mhatmaker@gmail.com";
            //var fromPassword = m_api.Credentials["GMAIL"].Secret;     //var fromPassword = "<gmail_password>";
            var smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromEmail, fromPassword)
            };
            var sms = new SmsPub(smtp, fromEmail, "CryptoTrader");
            
            sms.Send(Contacts.Hatmaker, CellularProvider.ATT, "Yo!", "This text message was sent from the TradeTronic API");

            //var email = new EmailPub(smtp, "mhatmaker@gmail.com", "CryptoTrader");
            //email.SendAsync("mhatmaker@yahoo.com", "trial using API", "This is a quick test of using email API");

            Console.WriteLine("Sent async. Waiting...");
            while (true)
            {
                Thread.Sleep(500);
            }

            //m_sms = new SmsPub("+12244791070", "ACf0f52caf17f5c0920139c863d528ea37", "c1c418f8e269cc1e62b4279c42525ee1");
            //m_sms.Send("+13125139500", "Sending you a test SMS message!");
            //m_sms.Send("+13129614029", "Sending you a test SMS message!");
        }

		/*
        Alltel: phonenumber@message.alltel.com.
        AT&T: phonenumber@txt.att.net.
        T-Mobile: phonenumber@tmomail.net.
        Virgin Mobile: phonenumber@vmobl.com.
        Sprint: phonenumber@messaging.sprintpcs.com.
        Verizon: phonenumber@vtext.com.
        Nextel: phonenumber@messaging.nextel.com.
        US Cellular: phonenumber@mms.uscc.net.
        */
        // where phoneNumber like "3125139500"
		public string GetSmsToEmail(string phoneNumber, CellularProvider provider)
		{
			string smsEmail = null;

			switch (provider)
			{
				case CellularProvider.Alltel:
					smsEmail = string.Format("{0}@message.alltel.com", phoneNumber);
					break;
				case CellularProvider.ATT:
                    smsEmail = string.Format("{0}@txt.att.net", phoneNumber);
                    break;
				case CellularProvider.Tmobile:
                    smsEmail = string.Format("{0}@tmomail.net", phoneNumber);
                    break;
				case CellularProvider.VirginMobile:
                    smsEmail = string.Format("{0}@vmobl.com", phoneNumber);
                    break;
				case CellularProvider.Sprint:
                    smsEmail = string.Format("{0}@messaging.sprintpcs.com", phoneNumber);
                    break;
				case CellularProvider.Verizon:
                    smsEmail = string.Format("{0}@vtext.com", phoneNumber);
                    break;
				case CellularProvider.Nextel:
                    smsEmail = string.Format("{0}@messaging.nextel.com", phoneNumber);
                    break;
				case CellularProvider.USCellular:
                    smsEmail = string.Format("{0}@mms.uscc.net", phoneNumber);
                    break;
			}
			return smsEmail;
		}

        public void Send(string phoneNumber, CellularProvider provider, string subject, string msg)
		{
			var toEmail = GetSmsToEmail(phoneNumber, provider);
			//m_email.SendAsync(toEmail, subject, msg);
            m_email.Send(toEmail, subject, msg);
        }


        /*private string m_twilioNumber;

		// CTOR: Twilio
		// where twilioNumber like "+12244791070"
        public SmsPub(string twilioNumber, string twilioAccountSid, string twilioAuthToken)
        {
			//var twilioAccountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
			//var twilioAuthToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
			TwilioClient.Init(twilioAccountSid, twilioAuthToken);
			m_twilioNumber = twilioNumber;
        }

        // where phoneNumber like "+13125139500"
		// where msg like Ahoy from Twilio!"
        public void Send(string phoneNumber, string msg)
		{
			MessageResource.Create(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(m_twilioNumber),
                body: msg);
		}*/

    } // end of class SmsPub

} // end of namespace
