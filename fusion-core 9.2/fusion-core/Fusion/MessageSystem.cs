using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Fusion.MessageSystem
{
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
            this.Name = Name;
            Commands = new Dictionary<string, Action>();
            Receivers.Add(this);
        }
        public MessageReceiver(string Name, Action<Message> ReceiveMethod)
        {
            if (Receivers == null)
            {
                Receivers = new HashSet<MessageReceiver>();
            }
            this.Name = Name;
            this.ReceiveMethod = ReceiveMethod;
            Commands = new Dictionary<string, Action>();
            Receivers.Add(this);
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
        public MessageSender(string Name)
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
        public void Preload(Message Message)
        {
            Message.Sender = this;
            PreloadedMessages.Add(Message);            
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
        public void SendTo(Message Message, MessageReceiver Receiver)
        {
            if(PreloadedMessages.Contains(Message) && Receivers.Contains(Receiver))
            {
                Receiver.ReceiveMessage(Message);
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
