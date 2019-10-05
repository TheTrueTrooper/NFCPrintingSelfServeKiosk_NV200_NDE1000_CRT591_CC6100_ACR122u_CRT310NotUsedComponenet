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
        
        /// <summary>
        /// A error message if the address 
        /// </summary>
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
        /// Clear the line command
        /// </summary>
        const byte EOT = 0x04;

        //ADDR Address char

        //CMT Command head 
        const byte CMT = 0x43;

        /// <summary>
        /// the buad Rate to which the machine communicates
        /// </summary>
        const int BaudRate = 192000; /*= 9600;*/ /*= 57600;*/ /*= 38400;*/

        /// <summary>
        /// The data byte size for this machine (standard hex)
        /// </summary>
        const int DataSize = 8;

        /// <summary>
        /// The machines Address
        /// </summary>
        public byte MachinesAddress { get; private set; }

        /// <summary>
        /// The actual Serial port for communication
        /// </summary>
        SerialPort SerialPort;

        /// <summary>
        /// Returns the Serial Ports Name
        /// </summary>
        string PortName
        {
            get
            {
                return SerialPort.PortName;
            }
        }


        CRT591_Com(string SerialPortName, byte MachinesAddress = 0)
        {
            //check if the address is a valid address that the machine can be set for and then set the address with the serial settings
            if (MachinesAddress > 15 || MachinesAddress < 0)
                throw new Exception(AddressingError);
            SerialPort = new SerialPort(SerialPortName, BaudRate, Parity.None, DataSize, StopBits.One);
            this.MachinesAddress = MachinesAddress;
        }

        void SendCommand(byte Command, byte SubCommand, byte[] CommandData)
        {
            //the Message XOR check (XOR all all bytes and check against the last byte) 
            byte XORCheck = 0;
            //the High byte start will alway be 4 as you have STX = 1, ADDR = 1, LENH = 1, LENL = 1, and finally the start of the message => 0-4
            byte LENH = 4; // during debug this could be source of error as we are not sure if they mean 0 based indexing but have assumed as so
            //the low byte location set as the end of the data
            byte LENL = (byte)(6 + CommandData.Length);

            //this will all ways be the data plus 3
            int MessageLength = LENL + 3;

            byte[] Message = new byte[MessageLength];

            //set the start of the frame
            Message[0] = STX;
            Message[1] = MachinesAddress;
            Message[2] = LENH;
            Message[3] = LENL;
            Message[4] = CMT;
            Message[5] = Command;
            Message[6] = SubCommand;
            //load all the variable data
            Array.Copy(CommandData, 0, Message, 7, CommandData.Length);
            //Set the end message at the end of the message
            Message[Message.Length - 2] = ETX;
            //Generate a XOR check sum from summing the message using a XOR opp and tack it on
            foreach (byte i in Message)
                XORCheck ^= i;
            Message[Message.Length - 1] = XORCheck;

            //Send that message
            SerialPort.Write(Encoding.ASCII.GetString(Message));
        }

        CRT591_BaseResponseMessage DecodeResponse(byte[] Message)
        {
            // Negative message Command Header
            const byte CHN = 0x45;
            // Positive message Command Header
            const byte CHP = 0x50;
            
            //build a set of varables we can set
            CRT591_MessageResponseStatus MessageStatus = CRT591_MessageResponseStatus.UnkownFormateAssumedNotFor;
            byte Command = 0x00;
            byte Param = 0x00;

            byte LENH = 0x04;
            byte LENL = 0x04;

            byte MessageAddress = 0x00;

            //if the message is not a message return unknown
            if (Message[0] != STX)
                return new CRT591_BaseResponseMessage(CRT591_MessageResponseStatus.UnkownFormateAssumedNotFor);
            //if the message is not for the machine return wrong target
            if (Message[1] != MachinesAddress)
                return new CRT591_BaseResponseMessage(CRT591_MessageResponseStatus.NotForThisInstance);

            //fill i the basics
            MessageAddress = Message[1];
            LENH = Message[2];
            LENL = Message[3];

            if (Message[4] == CHP)
                MessageStatus = CRT591_MessageResponseStatus.Positive;
            else if (Message[4] == CHN)
                MessageStatus = CRT591_MessageResponseStatus.Negative;

            Command = Message[5];
            Param = Message[6];

            if (MessageStatus == CRT591_MessageResponseStatus.Positive)
                return DecodePositiveResponse(new CRT591_BaseResponseMessage(MessageStatus, MachinesAddress, Command, Param), Message, LENH, LENL);
            else if (MessageStatus == CRT591_MessageResponseStatus.Negative)
                return DecodeNegativeResponse(new CRT591_BaseResponseMessage(MessageStatus, MachinesAddress, Command, Param), Message, LENH, LENL);

            return new CRT591_BaseResponseMessage();
        }

        CRT591_PositiveResponseMessage DecodePositiveResponse(CRT591_BaseResponseMessage BaseOfMessage, byte[] Message, byte LENH, byte LENL)
        {
            //ST0 CardStatus
            CRT591_CardStackStatus CardStatus = (CRT591_CardStackStatus)Message[7];
            //ST1 CardStack 
            CRT591_CardStackStatus StackStatus = (CRT591_CardStackStatus)Message[8];
            //ST2 Error bin status
            CTR591_ErrorCardBinStatus ErrorBinStatus = (CTR591_ErrorCardBinStatus)Message[9];

            byte[] Data = new byte[LENL - LENH - 3];
#warning get Data not done
            return new CRT591_PositiveResponseMessage(BaseOfMessage.MachineAddress, BaseOfMessage.Command, BaseOfMessage.Param, CardStatus, StackStatus, ErrorBinStatus, Data);
        }

        CRT591_NegativeResponseMessage DecodeNegativeResponse(CRT591_BaseResponseMessage BaseOfMessage, byte[] Message, byte LENH, byte LENL)
        {
            byte[] Data = new byte[LENL - LENH - 2];
#warning get Data not done
            return new CRT591_PositiveResponseMessage(BaseOfMessage.MachineAddress, BaseOfMessage.Command, BaseOfMessage.Param, , Data);
        }

        #region helpers
        CRT591_CardStackStatus GetCardStatus(byte In)
        {
            switch (In)
            {
                case 0x30:
                    return CRT591_CardStackStatus.CardStatus_NoCard;
                case 0x31:
                    return CRT591_CardStackStatus.CardStatus_CardInGate;
                case 0x32:
                    return CRT591_CardStackStatus.CardStatus_CardInRFPostion;
                default:
                    return CRT591_CardStackStatus.CardStatus_Unkown;
            }
        }

        CRT591_CardStackStatus GetStackStatus(byte In)
        {
            switch (In)
            {
                case 0x30:
                    return CRT591_CardStackStatus.StackStatus_NoCards;
                case 0x31:
                    return CRT591_CardStackStatus.StackStatus_FewCards;
                case 0x32:
                    return CRT591_CardStackStatus.StackStatus_FullOfCards;
                default:
                    return CRT591_CardStackStatus.StackStatus_Unkown;
            }
        }

        CTR591_ErrorCardBinStatus GetErrorBinStatus(byte In)
        {
            switch (In)
            {
                case 0x30:
                    return CTR591_ErrorCardBinStatus.ErrorCardBinStatus_NotFull;
                case 0x31:
                    return CTR591_ErrorCardBinStatus.ErrorCardBinStatus_Full;
                default:
                    return CTR591_ErrorCardBinStatus.ErrorCardBinStatus_Unkown;
            }
        }
        #endregion

        public void Dispose()
        {
            SerialPort?.Dispose();
        }

        ~CRT591_Com()
        {
            SerialPort?.Dispose();
        }
    }
}
