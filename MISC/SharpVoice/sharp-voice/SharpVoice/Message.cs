using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace SharpVoice
{
    [DataContract]
    public class Message
    {
        internal Voice connection;

        public enum Direction
        {
            Incoming = 10,
            Outgoing = 11
        }

        public void Archive(bool archive)
        {
            Voice.Archive(this.ID, archive);
        }

        public void Delete()
        {
            Delete(true);
        }

        public void Delete(bool trash)
        {
            //Moves this message to the Trash. Use message.delete(0) to move it out of the Trash.
            Voice.Delete(this.ID, trash);
        }

        public string Download()
        {
            return Download(System.IO.Path.Combine(Environment.CurrentDirectory, this.ID) + ".mp3");
        }

        public string Download(string adir)
        {
            //Download the message MP3 (if any). Saves files to adir (defaults to current directory). Message hashes can be found in self.voicemail().messages for example. Returns location of saved file.
            if (this.HasMP3 && connection != null)
            {
                return Voice.SaveVoicemail(this.ID, adir);
            }
            else if (!this.HasMP3)
            {
                throw new InvalidOperationException("No MP3 available");
            }
            return "";
        }

        public void MarkRead()
        {
            MarkRead(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection">Voice object</param>
        /// <param name="read">True = mark as read; False = mark as unread</param>
        public void MarkRead(bool read)
        {
            Voice.MarkRead(this.ID, read);
        }

        public override string ToString()
        {
            return ID;
        }

        [DataMember(Name = "id")]
        internal string id { get; set; }
        public string ID { get { return this.id; } }

        [DataMember(Name = "phoneNumber")]
        public string phoneNumber { get; set; }

        [DataMember(Name = "displayNumber")]
        public string DisplayNumber { get; private set; }

        [DataMember]
        public string startTime { get; private set; }

        [DataMember]
        public string displayStartDateTime { get; private set; }

        [DataMember]
        public string displayStartTime { get; private set; }

        [DataMember(Name = "relativeStartTime")]
        public string relativeStartTime { get; private set; }

        [DataMember(Name = "note")]
        public string Note { get; private set; }

        [DataMember(Name = "isRead")]
        private bool isRead { get; set; }
        public bool IsRead
        {
            get { return this.isRead; }
            set
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("messages", this.ID);
                data.Add("read", value ? "1" : "0");
                Voice.Request("mark", data);
                this.isRead = value;
            }
        }

        [DataMember(Name = "isSpam")]
        public bool IsSpam { get; private set; }

        [DataMember(Name = "isTrash")]
        public bool IsTrash { get; private set; }

        [DataMember(Name = "star")]
        internal bool star { get; set; }
        public bool Star
        {
            get { return this.star; }
            set
            {
                Dictionary<string,string> data = new Dictionary<string,string>();
                data.Add("messages",this.ID);
                data.Add("star", value ? "1" : "0");
                Voice.Request("star", data);
                this.star = value;
            }
        }

        [DataMember(Name = "messageText")]
        public string Text { get; private set; }

        [DataMember(Name = "labels")]
        public string[] Labels { get; private set; }

        [DataMember(Name = "hasMp3")]
        public bool HasMP3 { get; private set; }

        [DataMember(Name = "duration")]
        public int Duration { get; private set; }

        [DataMember(Name = "type")]
        public Direction Type { get; private set; }

        [DataMember(Name = "children")]
        public string Children { get; private set; }
    }
}
