using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace CardReader_CRT_591
{
    public class CRT591_Com : IDisposable
    {
        const string AddressingError = "Address is not a vaild address for the machine.\nThe machine can only be set via 4 dp switches in the back.\nThis only leaves 16 posible values (0-15).";

        /// <summary>
        /// Start of Text Message
        /// </summary>
        const byte STX = 0xF2;
        /// <summary>
        /// End of Text Message
        /// </summary>
        const byte ETX = 0x03;
        /// <summary>
        /// Positive acknologment of command
        /// </summary>
        const byte ACK = 0x06;
        /// <summary>
        /// Negatie acknologment of command
        /// </summary>
        const byte NAK = 0x15;
        /// <summary>
        /// Clear the line
        /// </summary>
        const byte EOT = 0x04;

        //ADDR Address char

        //CMT Command head 
        const byte CMT = 0x43;

        const int BaudeRate = 9600;
        const int DataSize = 8;

        public byte MachinesAddress { get; private set; }


        SerialPort SerialPort;



        string PortName
        {
            get
            {
                return SerialPort.PortName;
            }
        }


        CRT591Com(string SerialPortName, byte MachinesAddress = 0)
        {
            if (MachinesAddress > 15 || MachinesAddress < 0)
                throw new Exception(AddressingError);
            SerialPort = new SerialPort(SerialPortName, BaudeRate, Parity.None, DataSize, StopBits.One);
            this.MachinesAddress = MachinesAddress;
        }

        void SendCommand(byte Command, byte SubCommand, byte[] CommandData)
        {

            byte XORCheck = 0;
            byte LENH = 4; // during debug this could be source of error as we are not sure if they mean 0 based indexing but have assumed as so
            byte LENL = (byte)(6 + CommandData.Length);

            int MessageLength = LENL + 6;

            byte[] Message = new byte[MessageLength];

            Message[0] = STX;
            Message[1] = MachinesAddress;
            Message[2] = LENH;
            Message[3] = LENL;
            Message[4] = CMT;
            Message[5] = Command;
            Message[6] = SubCommand;
            Array.Copy(CommandData, 0, Message, 7, CommandData.Length);
            Message[Message.Length - 2] = ETX;
            foreach (byte i in Message)
                XORCheck ^= i;
            Message[Message.Length - 1] = XORCheck;

            SerialPort.Write(Encoding.ASCII.GetString(Message));
        }

        CRT591_MessageResponse DecodeResponse(byte[] Message)
        {
            /// <summary>
            /// Negative message Command Header
            /// </summary>
            const byte CHN = 0x45;
            /// <summary>
            /// Positive message Command Header
            /// </summary>
            const byte CHP = 0x50;

            CRT591_MessageResponseStatus Status;
            CRT591_MessageResponseCommandHeaderStatus CommandHeaderStatus = CRT591_MessageResponseCommandHeaderStatus.Unknown;
            if (Message[0] != STX)
                return new CRT591_MessageResponse();
            if (Message[1] != MachinesAddress)
                return new CRT591_MessageResponse(CRT591_MessageResponseStatus.NotForThisInstance);
            if (Message[4] == CHP)
                CommandHeaderStatus = CRT591_MessageResponseCommandHeaderStatus.Positive;
            else if (Message[4] == CHN)
                CommandHeaderStatus = CRT591_MessageResponseCommandHeaderStatus.Negative;

        }

        public void Dispose()
        {
            SerialPort?.Dispose();
        }

        ~CRT591Com()
        {
            SerialPort?.Dispose();
        }
    }
}
