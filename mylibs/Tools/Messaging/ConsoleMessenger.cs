using System;

namespace Tools.Messaging
{
    public class ConsoleMessenger : IMessenger
    {
        /*private static readonly string FG_ONLY = "\x1b[{0}m";
        private static readonly string BG_ONLY = "\x1b[{0}m";
        private static readonly string FG_BG = "\x1b[{0};{1}m";
        private static readonly string RESET = "\x1b[0m";*/

        public ConsoleMessenger()
        {
        }

        //+---------+------------+------------+
        //|  color  | foreground | background |
        //|         |    code    |    code    |
        //+---------+------------+------------+
        //| black   |     30     |     40     |
        //| red     |     31     |     41     |
        //| green   |     32     |     42     |
        //| yellow  |     33     |     43     |
        //| blue    |     34     |     44     |
        //| magenta |     35     |     45     |
        //| cyan    |     36     |     46     |
        //| white   |     37     |     47     |
        //+---------+------------+------------+

        //TelegramBot.Instance.SendToAll(msg);
        //TelegramBot.Instance.SendToAll(msg);

        public void Send(string msg)
        {
            Console.WriteLine(">>> " + msg);
        }

        /*public void SendText(string id, string msg, int? fgCode = null, int? bgCode = null)
        {
            string message = msg;
            if (fgCode != null && bgCode != null)
            {
                message = string.Format(FG_BG, fgCode.Value, bgCode.Value) + msg + RESET; 
            }
            else if (fgCode != null)
            {
                
            }
            else if (bgCode != null)
            {
                
            }
            Console.WriteLine("{0}> {1}", id, message);
        }

        private string FG(string msg, int fgCode)
        {
            string format = string.Format(FG_ONLY, fgCode);
            return format + msg + RESET;
        }*/

    } // end of class ConsoleMessenger

} // end of namespace
