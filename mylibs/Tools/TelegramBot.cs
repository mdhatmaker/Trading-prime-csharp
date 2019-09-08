using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;
//using TeleSharp.TL;
//using TLSharp;
//using TLSharp.Core;
using static Tools.G;

namespace Tools
{
    public class TelegramBot
    {
        public Telegram.Bot.Types.User WhoAmI { get; private set; }     // Returns the User information about the bot
        public bool AutoAddUsers = true;                                // Default to automatically adding "SendToAll" users from whom we get a message

        private HashSet<long> m_chatIds = new HashSet<long>();
        private TelegramBotClient m_bot;

        // CREATE PATTERN (?)
        public static TelegramBot Instance { get { return m_instance; } }
        private static TelegramBot m_instance;
        public static TelegramBot Create(string apiKey) { return new TelegramBot(apiKey); }
        private TelegramBot(string apiKey)
        {
            m_instance = this;

            m_bot = new TelegramBotClient(apiKey);

            WhoAmI = m_bot.GetMeAsync().Result;
            dout("Telegram BOT online: '{0}'", WhoAmI.Username);

            m_bot.OnMessage += Bot_OnMessage;
            m_bot.OnMessageEdited += Bot_OnMessageEdited;
            m_bot.OnCallbackQuery += Bot_OnCallbackQuery;
            m_bot.OnInlineQuery += Bot_OnInlineQuery;
            m_bot.OnInlineResultChosen += Bot_OnInlineResultChosen;
            m_bot.OnReceiveError += Bot_OnReceiveError;

            this.Start();

            /*Bot.StartReceiving();
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            Bot.StopReceiving();*/
        }


        #region -------------------------------- EVENT HANDLERS ---------------------------------------------------------------------------
        private void Bot_OnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            cout("Bot_OnReceiveError: {0}", e.ApiRequestException.Message);            
        }

        private void Bot_OnInlineResultChosen(object sender, ChosenInlineResultEventArgs e)
        {
            cout("Bot_OnInlineResultChosen");
        }

        private void Bot_OnInlineQuery(object sender, InlineQueryEventArgs e)
        {
            cout("Bot_OnInlineQuery");
        }

        private void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            cout("Bot_OnCallbackQuery");
        }

        private void Bot_OnMessageEdited(object sender, MessageEventArgs e)
        {
            cout("Bot_OnMessageEdited");
        }

        private void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            dout("Bot_OnMessage: '{0}'\n...from '{1}' '{2}' Chat.Username='{3}' (Chat.Id='{4}', From.Id='{5}')", e.Message.Text, e.Message.Chat.FirstName, e.Message.Chat.LastName, e.Message.Chat.Username, e.Message.Chat.Id, e.Message.From.Id);
            if (AutoAddUsers) m_chatIds.Add(e.Message.Chat.Id);
        }
        #endregion ------------------------------------------------------------------------------------------------------------------------


        // Start receiving updates (they are started by default in the constructor)
        public void Start()
        {
            m_bot.StartReceiving();
        }

        // Stop receiving updates
        public void Stop()
        {
            m_bot.StopReceiving();
        }

        // Explicitly add a chat id to which the "SendToAll" messages will be sent
        public void AddChatId(long id)
        {
            m_chatIds.Add(id);
        }

        // Explicitly remove a chat id from the "SendToAll" id list
        public void RemoveChatId(long id)
        {
            m_chatIds.Remove(id);
        }

        // Where user like "@aclifford"
        public void Send(string name, string message)
        {
            //var t = await m_bot.SendTextMessageAsync("@Hat&Alvin", "this is a test...of my TELEGRAM API CODE, bitches!");
            dout("Sending Telegram message to name: '{0}'", name);
            m_bot.SendTextMessageAsync(name, message);
        }

        // Send the given message to ALL chat ids the bot has accumulated (or that have been explicitly added)
        public void SendToAll(string message)
        {
            //var t = await m_bot.SendTextMessageAsync("@Hat&Alvin", "this is a test...of my TELEGRAM API CODE, bitches!");
            dout("Sending Telegram message to {0} chat id(s)", m_chatIds.Count);
            foreach (var id in m_chatIds)
            {
                m_bot.SendTextMessageAsync(id, message);
            }
        }

        // Given a chat id (long) and a message (string)
        // Send the message to the given chat id
        public void Send(long chatId, string message)
        {
            dout("Sending Telegram message to chat id: {0}", chatId);
            m_bot.SendTextMessageAsync(chatId, message);
        }

    } // end of class TelegramBot






    
    #region NO WORKO
    /*//=========================================DOESN'T SEEM TO WORK========================================================================
    // https://github.com/sochix/TLSharp
    public class TelegramMessaging
    {
        TelegramClient client;

        public TelegramMessaging()
        {
            client = new TelegramClient(105463, "527c65bbd48dc32c2bbaeaeb2922ad54");
            //await m_client.ConnectAsync();

            //var store = new FileSessionStore();
            //client = new TelegramClient(store, "session");
        }

        public bool Connect()
        {            
            return client.ConnectAsync().Result;
        }

        public async void Authenticate()
        {
            string userNumber = "+13125139500";
            //var hash = await m_client.SendCodeRequestAsync(userNumber);
            var hash = client.SendCodeRequestAsync(userNumber).Result;
            var code = "54801"; // you can change code in debugger
            //var authuser = await m_client.MakeAuthAsync(userNumber, hash, code);
            var authuser = client.MakeAuthAsync(userNumber, hash, code).Result;

            //get available contacts
            //var result = await m_client.GetContactsAsync();
            var result = client.GetContactsAsync().Result;

            // SEND MESSAGE TO FRIEND BY PHONE NUMBER
            //find recipient in contacts
            var user = result.Users
                .Where(x => x.GetType() == typeof(TLUser))
                .Cast<TLUser>()
                .FirstOrDefault(x => x.Phone == "+1 312 961 4029");
            //send message
            await client.SendMessageAsync(new TLInputPeerUser() { UserId = user.Id }, "OUR_MESSAGE");
            //await m_client.SendMessageAsync(new TLInputPeerUser() { UserId = 386868270 }, "Hey bro! I sent this message using the Telegram API!!!");
            
            //// SEND MESSAGE TO CHANNEL
            ////get user dialogs
            //TeleSharp.TL.Messages.TLAbsDialogs dialogs = await m_client.GetUserDialogsAsync();

            ////find channel by title            
            //var chat = dialogs.chats.lists
            //  .Where(c => c.GetType() == typeof(TLChannel))
            //  .Cast<TLChannel>()
            //  .FirstOrDefault(c => c.title == "HatBotTrades");
            ////send message
            //await m_client.SendMessageAsync(new TLInputPeerChannel() { ChannelId = chat.id, AccessHash = chat.access_hash.Value }, "OUR_MESSAGE");
            
        }
    }
    //=========================================DOESN'T SEEM TO WORK========================================================================
    */
    #endregion

} // end of namespace
