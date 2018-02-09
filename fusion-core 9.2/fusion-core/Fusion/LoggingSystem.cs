using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Fusion.MessageSystem;

namespace Fusion.LoggingSystems
{
    public interface ILogger
    {
        void Log(Message Message);
    }

    public class LocalLogger : ILogger
    {
        #region Fields
        TextWriter tw;
        MessageReceiver mr;
        #endregion
        #region Properties
        public MessageReceiver Receiver
        {
            get
            {
                return mr;
            }
        }
        #endregion
        #region Constructor/Destructor
        public LocalLogger()
        {
            Directory.CreateDirectory("logs");
            mr = new MessageReceiver("LocalLogger", ReceiveMessage);
            tw = new StreamWriter(Directory.GetCurrentDirectory() + @"\logs\" + DateTime.Now.ToFileTime() + ".txt");
        }
        ~LocalLogger()
        {
            tw.Close();
        }
        #endregion
        #region Methods
        // METHOD FOR MESSAGE RECEIVER
        void ReceiveMessage(Message message)
        {
            Log(message);
        }
        // METHODS FOR CONVERTING MESSAGES
        string ConvertMessage(Message Message)
        {
            return string.Format("({0})[{1}] - {2} ({3})", Message.Code, Message.isGood, Message.MessageContent, Message.Commands);
        }
        // METHOD THAT ACTUALLY WRITES IT DOWN
        public void Log(Message Message)
        {
            tw.WriteLineAsync(ConvertMessage(Message)).Start();
        }
        #endregion
    }

    #region not-implemented OnlineLogger
    // for now not implementing, gotta think of a log server to send all the information to
    /*
    public class OnlineLogger : ILogger
    {

    }
    */
    #endregion
}
