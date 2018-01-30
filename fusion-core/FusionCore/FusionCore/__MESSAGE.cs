using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fusion
{
    public class __MESSAGE
    {
        public uint Code;
        public bool Good;
        public string Instruction, Message;

        public __MESSAGE(bool Good, uint Code, string Message, string Instruction = "")
        {
            this.Good = Good;
            this.Code = Code;
            this.Instruction = Instruction;
            this.Message = Message;
        }
    }
    
    public class __MESSAGE_SENDER
    {
        public HashSet<__MESSAGE> SavedMessages;
        public __MESSAGE_RECEIVER DefaultDestination;

        public __MESSAGE_SENDER()
        {
            SavedMessages = new HashSet<__MESSAGE>();
            DefaultDestination = __MESSAGE_RECEIVER.Default;
        }

        public void SendMessageAsync(__MESSAGE toSend)
        {
            SendMessage(toSend).Wait();
        }

        private async Task SendMessage(__MESSAGE toSend, __MESSAGE_RECEIVER Destination = null)
        {
            if(Destination != null)
            {
                DefaultDestination = Destination;
            }
            await DefaultDestination.ReceiveTask(toSend);
        }
    }
    
    public class __MESSAGE_RECEIVER
    {
        public static __MESSAGE_RECEIVER Default;
        
        // THIS IS A CONSOLE METHOD
        public async Task ReceiveTask(__MESSAGE toSend)
        {
            TextWriter tw = new StreamWriter(Console.OpenStandardOutput());
            if(toSend.Good)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            await tw.WriteLineAsync("\n" + toSend.Code + "\n" + toSend.Message);
            Console.ResetColor();
            if(toSend.Instruction.Contains("--sd"))
            {
                Environment.Exit(1);
            }
        }
    }

    // SHOULD HAVE A SYSTEM OF PREMADE MESSAGES AND A WAY TO SAVE/LOAD THEM
    // SHOULD HAVE A SYSTEM OF RECEIVING AND HANDLING MESSAGES (tho this one should be more tied into the console)
}
