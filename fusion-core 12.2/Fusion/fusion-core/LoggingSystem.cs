using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Messaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Fusion.Core.Logging
{
    public interface ILogWriter
    {
        #region Properties
        MessageReceiver Receiver
        {
            get;
        }
        MessageSender Sender
        {
            get;
        }
        IEnumerable<Message> LoggerMessages
        {
            get;
        }
        #endregion
        #region Methods
        string MessageConvert(Message Message);
        void Log(Message Message);
        void Send(int code, MessageReceiver Receiver);
        void Send(Message Message, MessageReceiver Receiver);
        void SendToAll(int code);
        void SendToAll(Message Message);
        #endregion
    }

    
    public class LocalLogger : ILogWriter
    {
        #region Fields
        TextWriter MessageWriter;
        MessageReceiver MessageReceiver;
        MessageSender MessageSender;
        #endregion
        #region Properties
        public MessageSender Sender
        {
            get
            {
                return MessageSender;
            }
        }
        public MessageReceiver Receiver
        {
            get
            {
                return MessageReceiver;
            }
        }
        public IEnumerable<Message> LoggerMessages
        {
            get
            {
                return Sender.PreloadedMessages;
            }
        }
        public bool DebugMode
        {
            get
            {
                return ApplicationCore.Debug;
            }
        }
        public bool ConsoleExists
        {
            get
            {
                try
                {
                    int x = Console.WindowHeight;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        string Path
        {
            get
            {
                return Directory.GetCurrentDirectory() + @"\logs\" + DateTime.Now.ToFileTime() + ".txt";
            }
        }
        #endregion
        #region Constructors
        public LocalLogger()
        {
            MessageWriter = new StreamWriter(new FileStream(Path, FileMode.Append));
            MessageReceiver = Integrated.LogReceiver(Receive, ShutDown, HideConsole);
            MessageSender = Integrated.LogSender();
            if(DebugMode)
            {
                SendToAll(901);
            }
        }
        ~LocalLogger()
        {
            MessageWriter.Close();
        }
        #endregion
        #region Methods/Public
        public string MessageConvert(Message Message)
        {
            return Message.ToOneLine();
        }
        public void Log(Message Message)
        {
            MessageWriter.WriteLine(MessageConvert(Message));
        }
        public void Send(int code,MessageReceiver Receiver)
        {
            Sender.SendTo(code, Receiver);
        }
        public void Send(Message Message, MessageReceiver Receiver)
        {
            Sender.SendTo(Message, Receiver);
        }
        public void SendToAll(int code)
        {
            Sender.SendToAll(code);
        }
        public void SendToAll(Message Message)
        {
            Sender.SendToAll(Message);
        }
        #endregion
        #region Methods/Private
        void Receive(Message Message)
        {
            if(ConsoleExists && DebugMode)
            {
                Console.WriteLine(Message.ToOneLine());
            }
            Log(Message);
        }
        #region Thingsneededtocloseconsolewindow
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        #endregion
        void HideConsole()
        {
            if (!DebugMode && ConsoleExists)
            {
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);
            }            
        }
        void ShutDown()
        {
            if(!DebugMode)
            {
                Environment.Exit(101);
            }
        }
        #endregion
    }
}
