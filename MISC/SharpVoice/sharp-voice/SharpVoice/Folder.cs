using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections;
using System.Diagnostics;
using System.Collections.Specialized;

namespace SharpVoice
{
    [DataContract]
    public class Folder
    {
        /// <summary>
        /// Seconds to cache data
        /// </summary>
        private const int CACHE_TIME = 10;

        DateTime _LastUpdate = DateTime.MinValue;

        internal Voice voiceConnection;

        public DateTime LastUpdate
        {
            get { return _LastUpdate; }
            set { _LastUpdate = value; }
        }

        public bool NeedsUpdate
        {
            get { return ((DateTime.Now - _LastUpdate).TotalSeconds >= CACHE_TIME); }
        }

        public Message this[string id]
        {
            get
            {
                Debug.WriteLine("get:Message id");
                return MessageDict[id];
            }
            set
            {
                MessageDict[id] = value;
            }
        }

        public Message this[int index]
        {
            get
            {
                Debug.WriteLine("get:Message index");
                return MessageDict.Values.ElementAt(index);
            }
        }

        [DataMember(Name = "messages")]
        private SortedDictionary<string, Message> MessageDict { get; set; }
        
        public Message[] Messages
        {
            get
            {
                Debug.WriteLine("get:Messages");
                return MessageDict.Values.ToArray();
            }
        }

        [DataMember(Name="totalSize")]
        public int TotalSize { get; set; }

        [DataMember(Name="resultsPerPage")]
        public int ResultsPerPage { get; set; }

        [DataMember(Name="unreadCounts")]
        public Dictionary<string, int> UnreadCounts { get; set; }

        public const string ALL = "all";
        public const string UNREAD = "unread";
        public const string INBOX = "inbox";
        public const string SMS = "sms";
        public const string VOICEMAIL = "voicemail";

        public FolderType Type { get; set; }
    }

    public enum FolderType
    {
        All,
        Unread,
        Inbox,
        SMS,
        Voicemail
    }
}
