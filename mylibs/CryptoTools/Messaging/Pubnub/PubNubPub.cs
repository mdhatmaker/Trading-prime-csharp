using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using PubnubApi;

namespace CryptoTools.Messaging
{
    public class PubnubMessageReceivedEventArgs : EventArgs
    {
        public string Channel { get; set; }
        public object Message { get; set; }
        public string Subscription { get; set; }
        public long Timetoken { get; set; }

        public PubnubMessageReceivedEventArgs(string channel, object message, string subscription, long timetoken)
        {
            Channel = channel;
            Message = message;
            Subscription = subscription;
            Timetoken = timetoken;
        }
    }

    public class PubnubPub
    {
        public event Action<EventArgs> PubnubConnected;
        public event Action<PubnubMessageReceivedEventArgs> PubnubMessageReceived;

        private Pubnub m_pubnub;

        public PubnubPub(string subscribeKey, string publishKey)
        {
            PNConfiguration config = new PNConfiguration
            {
                SubscribeKey = subscribeKey,
                PublishKey = publishKey
            };

            m_pubnub = new Pubnub(config);
            m_pubnub.AddListener(new SubscribeCallbackExt(
                HandleMessageCallback,
                (pubnubObj, presence) => { },
                HandleStatusCallback
            ));
        }

        private void HandleMessageCallback(Pubnub pubnubObj, PNMessageResult<object> message)
        {
            // Handle new message stored in message.Message
            if (message != null)
            {
                if (message.Channel != null)
                {
                    // Message has been received on channel group stored in message.Channel
                    //Console.WriteLine("[{0}] message.Channel: {1} {2} {3}", message.Channel, message.Message, message.Subscription, message.Timetoken);
                    var args = new PubnubMessageReceivedEventArgs(message.Channel, message.Message, message.Subscription, message.Timetoken);
                    PubnubMessageReceived?.Invoke(args);
                }
                else
                {
                    // Message has been received on channel stored in message.Subscription
                    //Console.WriteLine("[{0}] message.Subscription: {1} {2} {3}", message.Subscription, message.Message, message.Subscription, message.Timetoken);
                    var args = new PubnubMessageReceivedEventArgs(message.Subscription, message.Message, message.Subscription, message.Timetoken);
                    PubnubMessageReceived?.Invoke(args);
                }
                /*
                        log the following items with your favorite logger
                            - message.Message()
                            - message.Subscription()
                            - message.Timetoken()
                */
            }
        }

        //private void PresenceCallback(Pubnub pubnubObj, PNPresenceEventResult presence) { }

        private void HandleStatusCallback(Pubnub pubnubObj, PNStatus status)
        {
            if (status.Category == PNStatusCategory.PNUnexpectedDisconnectCategory)
            {
                // This event happens when radio / connectivity is lost
            }
            else if (status.Category == PNStatusCategory.PNConnectedCategory)
            {
                // Connect event. You can do stuff like publish, and know you'll get it.
                // Or just use the connected event to confirm you are subscribed for
                // UI / internal notifications, etc.
                PubnubConnected?.Invoke(EventArgs.Empty);
            }
            else if (status.Category == PNStatusCategory.PNReconnectedCategory)
            {
                // Happens as part of our regular operation. This event happens when
                // radio / connectivity is lost, then regained.
            }
            else if (status.Category == PNStatusCategory.PNDecryptionErrorCategory)
            {
                // Handle message decryption error. Probably client configured to
                // encrypt messages and on live data feed it received plain text.
            }
        }

        // where channels like new string[] { "awesomeChannel" }
        public void Subscribe(string[] channels)
        {
            m_pubnub.Subscribe<string>()
              .Channels(channels)
              .Execute();
        }

        // where channel like "awesomeChannel" and message like "Hello!!!"
        public void Publish(string channel, string message)
        {
            m_pubnub.Publish()
            .Channel(channel)
            .Message(message)
            .Async(new PNPublishResultExt((publishResult, publishStatus) =>
            {
                // Check whether request successfully completed or not
                if (!publishStatus.Error)
                {
                    // Message successfully published to specified channel
                }
                else
                {
                    // Request processing failed.

                    // Handle message publish error. Check 'Category' property to find out possible
                    // issue which caused request to fail.
                    Console.WriteLine("[PUBNUB ERROR] category:{0}", publishStatus.Category);
                }
            }));        
        }


        public void Test_original_code()
        {
            PNConfiguration config = new PNConfiguration
            {
                SubscribeKey = "demo",
                PublishKey = "demo"
            };

            m_pubnub = new Pubnub(config);
            m_pubnub.AddListener(new SubscribeCallbackExt(
                (pubnubObj, message) =>
                {
                    // Handle new message stored in message.Message
                    if (message != null)
                    {
                        if (message.Channel != null)
                        {
                            // Message has been received on channel group stored in message.Channel()
                        }
                        else
                        {
                            // Message has been received on channel stored in message.Subscription()
                        }

                        /*
                                log the following items with your favorite logger
                                    - message.Message()
                                    - message.Subscription()
                                    - message.Timetoken()
                        */
                    }
                },
                (pubnubObj, presence) => { },
                (pubnubObj, status) =>
                {
                    if (status.Category == PNStatusCategory.PNUnexpectedDisconnectCategory)
                    {
                        // This event happens when radio / connectivity is lost
                    }
                    else if (status.Category == PNStatusCategory.PNConnectedCategory)
                    {
                        // Connect event. You can do stuff like publish, and know you'll get it.
                        // Or just use the connected event to confirm you are subscribed for
                        // UI / internal notifications, etc.

                        m_pubnub.Publish()
                            .Channel("awesomeChannel")
                            .Message("hello!!!")
                            .Async(new PNPublishResultExt((publishResult, publishStatus) =>
                            {
                            // Check whether request successfully completed or not
                            if (!publishStatus.Error)
                            {
                                // Message successfully published to specified channel
                            }
                                else
                                {
                                // Request processing failed.

                                // Handle message publish error. Check 'Category' property to find out possible
                                // issue because of which request did fail.
                            }
                            }));
                    }
                    else if (status.Category == PNStatusCategory.PNReconnectedCategory)
                    {
                        // Happens as part of our regular operation. This event happens when
                        // radio / connectivity is lost, then regained.
                    }
                    else if (status.Category == PNStatusCategory.PNDecryptionErrorCategory)
                    {
                        // Handle message decryption error. Probably client configured to
                        // encrypt messages and on live data feed it received plain text.
                    }
                }
            ));

            m_pubnub.Subscribe<string>()
                .Channels(new string[]
                {
                    "awesomeChannel"
                })
                .Execute();

        }

    } // end of class PubNubPub

} // end of namespace
