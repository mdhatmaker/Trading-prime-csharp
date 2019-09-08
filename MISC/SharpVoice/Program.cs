using System;// I'm pretty sure this is required...
using System.Collections.Generic;// I don't think i used any collections
using System.Linq;// probably not needed at all.
using System.Windows.Forms;//Console app, but I like to show message boxes for errors...
using System.IO;// this is needed.
using System.Threading;// yay threads!
using System.Diagnostics;//Helpful in getting this code working.
//using System.Xml;//not needed unless you want to try parsing the get*() functions.

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
            // Give me your username
            Console.WriteLine("Enter Your Google Voice Username, eg user@gmail.com:");
            String userName = null;
            try {

                userName = Console.ReadLine();
            } catch {
                Console.WriteLine("IO error trying to read your name!");
                return;
            }

            // And your password
            Console.WriteLine("Enter Your Password:");
            String pass = null;
            try {
                pass = Console.ReadLine();
            } catch {
                 Console.WriteLine("IO error trying to read your name!");
                 return;
            }

            // I honestly don't have a clue what this if for...
            // it was in the java code, so I left it but I don't think it's used for anything.
            Console.WriteLine("Enter A \"Source\" for the Log:");
            String source = null;
            try {
                source = Console.ReadLine();
            } catch {
                 Console.WriteLine("IO error trying to read your name!");
                 return;
            }

            // the really long string you have to get from the google voice site source.
            Console.WriteLine("Log into Google Voice and find the _rnr_se variable in the page Source. ");
            Console.WriteLine("Enter rnr_se_ value:");
            String rnrSee = null;
            try {
                rnrSee = Console.ReadLine();
            } catch {
                 Console.WriteLine("IO error trying to read your name!");
                 return;
            }

            try {
                
                //Time to make the voice connection object!
                Voice voice = new Voice(userName, pass, source, rnrSee);

                try {
                    
                    // This code is for sending text messages...
                    Console.Write("Number: ");
                    string number = Console.ReadLine();
                    Console.Write("Message: ");
                    string msg = Console.ReadLine();

                    MessageBox.Show( voice.sendSMS(number, msg));
                    

                    /*
                    // This code is for getting the XML data from google about
                    // information in your inboxes. 
                    // Parse it yourself, I'm too lazy to do it.
                    
                    
                    File.WriteAllText("inbox.log", voice.getInbox());
                    Thread.Sleep(2000);
                    
                    File.WriteAllText("missed.log",voice.getMissed());
                    Thread.Sleep(2000);
                    
                    File.WriteAllText("placed.log",voice.getPlaced());
                    Thread.Sleep(2000);
                    
                    File.WriteAllText("received.log",voice.getReceived());
                    Thread.Sleep(2000);
                    
                    File.WriteAllText("recent.log",voice.getRecent());
                    Thread.Sleep(2000);
                    
                    File.WriteAllText("recorded.log",voice.getRecorded());
                    Thread.Sleep(2000);
                    
                    File.WriteAllText("sms.log",voice.getSMS());
                    Thread.Sleep(2000);
                    
                    File.WriteAllText("spam.log",voice.getSpam());
                    Thread.Sleep(2000);
                    
                    File.WriteAllText("starred.log",voice.getStarred());
                    Thread.Sleep(2000);
                    
                	*/



                } catch( Exception e) {
                    Debug.WriteLine(e.StackTrace);
                    MessageBox.Show(e.Message);//Debug help
                }
		    } catch (IOException e) {
                Debug.WriteLine(e.StackTrace);
                MessageBox.Show(e.Message);//more Debug help
		    }
        }
    }
}
