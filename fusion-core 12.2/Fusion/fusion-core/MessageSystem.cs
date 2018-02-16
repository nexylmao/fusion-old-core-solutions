using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Fusion.Core.Database;

namespace Fusion.Core.Messaging
{
    public static class Integrated
    {
        #region Core
        public static MessageSender CoreSender()
        {
            MessageSender x = new MessageSender("Core", MessageReceiver.ReceiverList);
            x.Preload(CoreMessages());
            return x;
        }

        public static IEnumerable<Message> CoreMessages()
        {
            HashSet<Message> x = new HashSet<Message>();
            x.Add(new Message(false, 103, "Critical app error!", "--sd"));
            x.Add(new Message(true, 102, "App done executing!", "--sd"));
            return x;
        }
        #endregion
        #region Logger
        public static MessageReceiver LogReceiver(Action<Message> receive, Action shutdown, Action hideconsole)
        {
            MessageReceiver MessageReceiver = new MessageReceiver("LocalLogger", receive);
            MessageReceiver.AddCommand("--sd", shutdown);
            MessageReceiver.AddCommand("--app-good", hideconsole);
            return MessageReceiver;
        }

        public static MessageSender LogSender()
        {
            MessageSender x = new MessageSender("Logger", MessageReceiver.ReceiverList);
            x.Preload(LogMessages());
            return x;
        }

        public static IEnumerable<Message> LogMessages()
        {
            HashSet<Message> x = new HashSet<Message>();
            x.Add(new Message(true, 901, "App running in debug mode!"));
            x.Add(new Message(true, 902, "Log write message to console"));
            x.Add(new Message(true, 101, "DefaultLogger is up!", "--app-good"));
            x.Add(new Message(true, 102, "App done executing!", "--sd"));
            x.Add(new Message(false, 303, "DLLManager doesn't have what to load!"));
            return x;
        }
        #endregion
        #region Database
        public static MessageSender DatabaseSender(string name)
        {
            MessageSender x = new MessageSender(name, MessageReceiver.ReceiverList);
            x.Preload(DatabaseMessages());
            return x;
        }
        
        public static IEnumerable<Message> DatabaseMessages()
        {
            HashSet<Message> x = new HashSet<Message>();
            x.Add(new Message(true, 201, "Loaded the database successfully!"));
            x.Add(new Message(false, 202, "Couldn't load the database!"));
            x.Add(new Message(true, 203, "Saved the database successfully!"));
            x.Add(new Message(false, 204, "Couldn't save the database!"));
            x.Add(new Message(true, 205, "Deleted the database successfully!"));
            x.Add(new Message(false, 206, "Couldn't delete the database! "));
            return x;
        }
        #endregion
        #region AssemblyLoader
        public static MessageReceiver AssemblyLoaderReceiver()
        {
            return new MessageReceiver("AssemblyLoader");
        }

        public static MessageSender AssemblyLoaderSender()
        {
            MessageSender Sender = new MessageSender("DLLManager", new MessageReceiver[] { MessageReceiver.First });
            Sender.Preload(AssemblyLoaderMessages());
            return Sender;
        }

        public static IEnumerable<Message> AssemblyLoaderMessages()
        {
            HashSet<Message> x = new HashSet<Message>();
            x.Add(new Message(false, 302, "DLLManager can't load DLL's!", "--sd"));
            x.Add(new Message(true, 301, "DLLManager loaded DLL's!"));
            x.Add(new Message(false, 303, "DLLManager doesn't have anything to load!"));
            x.Add(new Message(false, 304, "Error occured with unloading the domain!"));
            return x;
        }
        #endregion
    }

    public class Message : StorableObject
    {
        #region Fields
        public string sender;
        public bool isGood;
        public uint Code;
        public string MessageContent;
        public string Commands;
        #endregion
        #region Property
        [JsonIgnore]
        public MessageSender Sender
        {
            get
            {
                return MessageSender.FindSender(sender);
            }
            set
            {
                sender = value.Name;
            }
        }
        #endregion
        #region Constructors
        public Message(bool isGood)
        {
            this.isGood = isGood;
        }
        public Message(uint Code)
        {
            this.Code = Code;
        }
        public Message(string MessageContent)
        {
            this.MessageContent = MessageContent;
        }
        public Message(string MessageContent, string Commands)
        {
            this.MessageContent = MessageContent;
            this.Commands = Commands;
        }
        public Message(bool isGood, uint Code)
        {
            this.isGood = isGood;
            this.Code = Code;
        }
        public Message(bool isGood, uint Code, string MessageContent)
        {
            this.isGood = isGood;
            this.Code = Code;
            this.MessageContent = MessageContent;
        }
        public Message(bool isGood, uint Code, string MessageContent, string Commands)
        {
            this.isGood = isGood;
            this.Code = Code;
            this.MessageContent = MessageContent;
            this.Commands = Commands;
        }
        #endregion
        #region Methods
        public void AddExceptionMessage(Exception ex)
        {
            MessageContent += " " + ex.Message;
        }
        public string ToJSON(Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }
        public string ToOneLine()
        {
            string buffer = string.Empty;
            if(isGood)
            {
                buffer += "++";
            }
            else
            {
                buffer += "--";
            }
            buffer += ("(" + Code + ") " + MessageContent + " " + Commands);
            return buffer;
        }
        #endregion
    }

    public class MessageReceiver : StorableObject
    {
        #region Fields
        public string Name;
        public Action<Message> ReceiveMethod;
        public Dictionary<string, Action> Commands;
        static HashSet<MessageReceiver> Receivers;
        #endregion
        #region Properties
        public static MessageReceiver First
        {
            get
            {
                if (Receivers == null)
                {
                    return null;
                }
                return Receivers.ElementAt(0);
            }
        }
        public static IEnumerable<MessageReceiver> ReceiverList
        {
            get
            {
                if(Receivers == null)
                {
                    Receivers = new HashSet<MessageReceiver>();
                }
                return Receivers;
            }
        }
        #endregion
        #region Constructors
        public MessageReceiver(string Name)
        {
            if(Receivers == null)
            {
                Receivers = new HashSet<MessageReceiver>();
            }
            Receivers.Add(this);
            this.Name = Name;
            Commands = new Dictionary<string, Action>();
        }
        public MessageReceiver(string Name, Action<Message> ReceiveMethod)
        {
            if (Receivers == null)
            {
                Receivers = new HashSet<MessageReceiver>();
            }
            Receivers.Add(this);
            this.Name = Name;
            this.ReceiveMethod = ReceiveMethod;
            Commands = new Dictionary<string, Action>();
        }
        #endregion
        #region Methods
        public static MessageReceiver Receiver(int index)
        {
            try
            {
                return Receivers.ElementAt(index);
            }
            catch
            {
                return null;
            }
        }
        public void SetReceiveMethod(Action<Message> ReceiveMethod)
        {
            this.ReceiveMethod = ReceiveMethod;
        }
        public void AddCommand(string Command, Action Action)
        {
            Commands.Add(Command, Action);
        }
        public void ReceiveMessage(Message Message)
        {
            ReceiveMethod(Message);
            foreach(string x in Commands.Keys.ToList())
            {
                if(Message.Commands != null && Message.Commands.Contains(x))
                {
                    Commands.Values.ToList().ElementAt(Commands.Keys.ToList().IndexOf(x))();
                }
            }
        }
        #endregion
    }

    public class MessageSender : StorableObject
    {
        #region Fields
        public string Name;
        public HashSet<Message> PreloadedMessages;
        public HashSet<MessageReceiver> Receivers;
        static public LinkedList<MessageSender> Senders;
        #endregion
        #region Constructor
        public MessageSender(string Name, MessageReceiver first)
        {
            this.Name = Name;
            Receivers = new HashSet<MessageReceiver>();
            PreloadedMessages = new HashSet<Message>();
            if(Senders == null)
            {
                Senders = new LinkedList<MessageSender>();
            }
            Senders.AddLast(this);
        }
        public MessageSender(string Name, IEnumerable<MessageReceiver> Receiver)
        {
            this.Name = Name;
            Receivers = new HashSet<MessageReceiver>(Receiver);
            PreloadedMessages = new HashSet<Message>();
            if (Senders == null)
            {
                Senders = new LinkedList<MessageSender>();
            }
            Senders.AddLast(this);
        }
        #endregion
        #region Methods
        public static void SendQuick(Message Message)
        {
            MessageReceiver.First.ReceiveMessage(Message);
        }
        public static void SendQuick(Message Message, MessageReceiver Receiver)
        {
            Receiver.ReceiveMessage(Message);
        }
        public static MessageSender FindSender(string Name)
        {
            if(Senders != null)
            {
                foreach(MessageSender x in Senders)
                {
                    if(x.Name == Name)
                    {
                        return x;
                    }
                }
            }
            return null;
        }
        public Message Preload(Message Message)
        {
            Message.Sender = this;
            PreloadedMessages.Add(Message);
            return Message;
        }
        public void Preload(IEnumerable<Message> Messages)
        {
            if(Messages != null)
            {
                foreach (Message msg in Messages)
                {
                    Preload(msg);
                }
            }
        }
        public void Preload(Database<Message> Messages)
        {
            if(Messages != null)
            {
                for (int i = 0; i < Messages.Count; i++)
                {
                    Preload(Messages[i]);
                }
            }
        }
        public void DeletePreload(Message Message)
        {
            if(PreloadedMessages.Contains(Message))
            {
                PreloadedMessages.Remove(Message);
            }
        }
        public void DeletePreload(int index)
        {
            if(index < PreloadedMessages.Count && index >= 0)
            {
                PreloadedMessages.Remove(PreloadedMessages.ElementAt(index));
            }
        }
        public void AddReceiver(MessageReceiver newReceiver)
        {
            Receivers.Add(newReceiver);
        }
        public void DeleteReceiver(MessageReceiver Receiver)
        {
            if(Receivers.Contains(Receiver))
            {
                Receivers.Remove(Receiver);
            }
        }
        public void DeleteReceiver(int index)
        {
            if(index >= 0 && Receivers.Count > index)
            {
                Receivers.Remove(Receivers.ElementAt(index));
            }
        }
        public void SendTo(int code, MessageReceiver Receiver)
        {
            foreach(Message x in PreloadedMessages)
            {
                if(x.Code == code)
                {
                    SendTo(x, Receiver);
                }
            }
        }
        public void SendTo(Message Message, MessageReceiver Receiver)
        {
            if(PreloadedMessages.Contains(Message) && Receivers.Contains(Receiver))
            {
                Receiver.ReceiveMessage(Message);
            }
        }
        public void SendToAll(int code)
        {
            foreach(Message x in PreloadedMessages)
            {
                if(x.Code == code)
                {
                    SendToAll(x);
                }
            }
        }
        public void SendToAll(Message Message)
        {
            foreach(MessageReceiver one in Receivers)
            {
                one.ReceiveMessage(Message);
            }
        }
        #endregion
    }
}
