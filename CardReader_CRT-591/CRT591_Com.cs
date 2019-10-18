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
        enum Commands
        {
            INITIALIZE = 0x30,
            STATUSREQUEST = 0x31,
            CARDMOVE = 0x32,
            CARDENTRY = 0x33,
            CARDTYPE = 0x50,
            CPUCARDCONTROL = 0x51,
            SAMCARDCONTROL = 0x52,
            SLE4442_4428CARDCONTROL = 0x53,
            ICMEMORYCARD = 0x54,
            RFCARDCONTROL = 0x60,
            CARDSERIALNUMBER = 0xA2,
            READCARDCONFIG = 0xA3,
            READCRT591MVERSION = 0xA4,
            RECYCLEBINCOUNTER = 0xA5
        }

        bool Initialized = false;

        /// <summary>
        /// A error message if the address 
        /// </summary>
        const string AddressingError = "Address is not a vaild address for the machine.\nThe machine can only be set via 4 dp switches in the back.\nThis only leaves 16 posible values (0-15).";

        const string InitError = "Reader has not been Initialized. Please call 'SendResetInitCommand' to Initialize before use.";

        /// <summary>
        /// Start of Text Message
        /// </summary>
#if DEBUG
        public const byte STX = 0xF2;
#else
        const byte STX = 0xF2;
#endif

        /// <summary>
        /// End of Text Message
        /// </summary>
#if DEBUG
        public const byte ETX = 0x03;
#else
        const byte ETX = 0x03;
#endif

        /// <summary>
        /// Positive acknologment of command
        /// </summary>
#if DEBUG
        public const byte ACK = 0x06;
#else
        const byte ACK = 0x06;
#endif

        /// <summary>
        /// Negatie acknologment of command
        /// </summary>
#if DEBUG
        public const byte NAK = 0x15;
#else
        const byte NAK = 0x15;
#endif

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
#if DEBUG
        public const int BaudRate /*= 192000;*/ = 9600; /*= 57600;*/ /*= 38400;*/
#else
        const int BaudRate /*= 192000;*/ = 9600; /*= 57600;*/ /*= 38400;*/
#endif

        /// <summary>
        /// The data byte size for this machine (standard hex)
        /// </summary>
#if DEBUG
        public const int DataSize = 8;
#else
        const int DataSize = 8;
#endif

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


        public CRT591_Com(string SerialPortName, byte MachinesAddress = 0)
        {
            //check if the address is a valid address that the machine can be set for and then set the address with the serial settings
            if (MachinesAddress > 15 || MachinesAddress < 0)
                throw new Exception(AddressingError);
            SerialPort = new SerialPort(SerialPortName, BaudRate, Parity.None, DataSize, StopBits.One);
            this.MachinesAddress = MachinesAddress;
        }

        public void OpenCom()
        {
            SerialPort.Open();
        }

        #region Commands
        #region BaseSendCommand
        CRT591_PositiveResponseMessage SendCommand(byte Command, byte SubCommand, byte[] CommandData)
        {
            //the Message XOR check (XOR all all bytes and check against the last byte) 
            byte XORCheck = 0;
            //the High byte start will alway be 0
            byte LENH = 0;
            //this will aways be the length of the extra attached data plus the 3 always included data for the command
            byte LENL = (byte)(CommandData.Length + 3);
            //this will all ways be the data plus 6 for frame data
            int MessageLength = LENL + 6;

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
            for (byte i = 0; i < Message.Length - 1; i++)
                XORCheck ^= Message[i];
            Message[Message.Length - 1] = XORCheck;

            //Send that message
            SerialPort.Write(Message, 0, Message.Length);
            


            byte Ack = (byte)SerialPort.ReadByte();

            if (Ack == NAK)
                throw new Exception("Error the machine has Returned NAK and the request can not be handled at this time.");
            else if (Ack != ACK)
                throw new Exception("Error unexpected value recived for ACK and NAK");

            List<byte> Response = new List<byte>();
            byte Char;
            do
            {
                Char = (byte)SerialPort.ReadByte();
                Response.Add(Char);
            }
            while (Response.Count < 1 || Response[Response.Count - 1] != 0x03);
            Char = (byte)SerialPort.ReadByte();
            Response.Add(Char);
            CRT591_BaseResponseMessage Return = DecodeResponse(Response.ToArray());
            if (Return is CRT591_NegativeResponseMessage)
                throw new CRT591_CommandException((CRT591_NegativeResponseMessage)Return);
            return (CRT591_PositiveResponseMessage)Return;
        }
        #endregion

        public void SendResetInitCommand(out string RevType, CRT591_Commands_InitParam InItParam = CRT591_Commands_InitParam.DontMoveCard)
        {
            byte command = (byte)Commands.INITIALIZE;
            byte commandParameter = (byte)InItParam;
            byte[] commandData = new byte[0];

            CRT591_PositiveResponseMessage Return = SendCommand(command, commandParameter, commandData);
            RevType = ASCIIEncoding.ASCII.GetString(Return.DataRaw);

            Initialized = true;
        }

        public void SendResetInitCommand(CRT591_Commands_InitParam InItParam = CRT591_Commands_InitParam.DontMoveCard)
        {
            string Out;
            SendResetInitCommand(out Out, InItParam);
        }

        public void MoveCardCommand(CRT591_Commands_MoveCardParam MoveCardParma = CRT591_Commands_MoveCardParam.MoveCardToGate)
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.CARDMOVE;
            byte commandParameter = (byte)MoveCardParma;
            byte[] commandData = new byte[0];

            SendCommand(command, commandParameter, commandData);
        }

        public CRT591_PositiveResponseMessage CardGateEntrySetCommand(CRT591_Commands_SetCardEntryParam GateSetting = CRT591_Commands_SetCardEntryParam.DisableCardEntryFromOutput)
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.CARDENTRY;
            byte commandParameter = (byte)GateSetting; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        public CRT591_SensorStatus[] RequestStatus(CRT591_Commands_GetStatusParam StatusParma = CRT591_Commands_GetStatusParam.GetCRT591Status)
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.STATUSREQUEST;
            byte commandParameter = (byte)StatusParma; //Disable card input from gate
            byte[] commandData = new byte[0];
            CRT591_PositiveResponseMessage Return = SendCommand(command, commandParameter, commandData);
            CRT591_SensorStatus[] Sensors = new CRT591_SensorStatus[Return.DataRaw.Length];
            for (int i = 0; i < Return.DataRaw.Length; i++)
                Sensors[i] = (CRT591_SensorStatus)Return.DataRaw[i];
            return Sensors;
        }

        public int ResetAndReadBinCounter(CRT591_Commands_RecycleBinCounterParam BinParam = CRT591_Commands_RecycleBinCounterParam.ReadCounter)
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.RECYCLEBINCOUNTER;
            byte commandParameter = (byte)BinParam; //Disable card input from gate
            byte[] commandData = new byte[0];
            byte[] CountBytes = new byte[4];

            CRT591_PositiveResponseMessage Return = SendCommand(command, commandParameter, commandData);

            if (BitConverter.IsLittleEndian)
                Array.Copy(Return.DataRaw, CountBytes, Return.DataRaw.Length);
            else
                Array.Copy(Return.DataRaw, 1, CountBytes, 0, Return.DataRaw.Length);

            return BitConverter.ToInt32(CountBytes, 0);
        }

        public CRT591_PositiveResponseMessage ReadCRT591FirmwareVers()
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.READCRT591MVERSION;
            byte commandParameter = 0x30; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        public CRT591_PositiveResponseMessage ReadCardConfig()
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.READCARDCONFIG;
            byte commandParameter = 0x30; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        public CRT591_PositiveResponseMessage ReadCardSerialNumber()
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.CARDSERIALNUMBER;
            byte commandParameter = 0x30; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        public CRT591_PositiveResponseMessage ReadCardType(CRT591_Commands_CheckTypeRForICParam CheckTypeParam = CRT591_Commands_CheckTypeRForICParam.AutoCheckICType)
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.CARDTYPE;
            byte commandParameter = (byte)CheckTypeParam; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        internal CRT591_PositiveResponseMessage SendRFCardControl(CRT591_Commands_MifareRFOperationParam CardOperation = CRT591_Commands_MifareRFOperationParam.Startup)
        {
            byte command = (byte)Commands.RFCARDCONTROL;
            byte commandParameter = (byte)CardOperation; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        internal CRT591_PositiveResponseMessage SendCPUCardControl(CRT591_Commands_CPUOperationParam CardOperation = CRT591_Commands_CPUOperationParam.ColdReset)
        {
            byte command = (byte)Commands.CPUCARDCONTROL;
            byte commandParameter = (byte)CardOperation; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        internal CRT591_PositiveResponseMessage SendSAMCardControl(CRT591_Commands_SAMOperationParam CardOperation = CRT591_Commands_SAMOperationParam.ColdReset)
        {
            byte command = (byte)Commands.SAMCARDCONTROL;
            byte commandParameter = (byte)CardOperation; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        internal CRT591_PositiveResponseMessage SendICCardControl(CRT591_Commands_SAMOperationParam CardOperation = CRT591_Commands_SAMOperationParam.ColdReset)
        {
            byte command = (byte)Commands.ICMEMORYCARD;
            byte commandParameter = (byte)CardOperation; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        internal CRT591_PositiveResponseMessage SendSLE4442_4428CardControl(CRT591_Commands_SAMOperationParam CardOperation = CRT591_Commands_SAMOperationParam.ColdReset)
        {
            byte command = (byte)Commands.SLE4442_4428CARDCONTROL;
            byte commandParameter = (byte)CardOperation; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }
        #endregion

        #region MessageDecoding
        #region BaseMessageDecode
        CRT591_BaseResponseMessage DecodeResponse(byte[] Message)
        {
            //byte XORCheck = 0;
            //for (byte i = 0; i < Message.Length; i++)
            //    XORCheck ^= i;
            //XORCheck

            // Negative message Command Header
            const byte CHN = 0x45;
            // Positive message Command Header
            const byte CHP = 0x50;
            
            //build a set of varables we can set
            CRT591_MessageResponseStatus MessageStatus = CRT591_MessageResponseStatus.UnkownFormateAssumedNotFor;
            byte Command = 0x00;
            byte Param = 0x00;

            byte LENH = 0x00;
            byte LENL = 0x03;

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

            CRT591_BaseResponseMessage Base = new CRT591_BaseResponseMessage(MessageStatus, MachinesAddress, Command, Param);

            if (MessageStatus == CRT591_MessageResponseStatus.Positive)
                return DecodePositiveResponse(Base, LENH, LENL, Message);
            else if (MessageStatus == CRT591_MessageResponseStatus.Negative)
                return DecodeNegativeResponse(Base, LENH, LENL, Message);

            return new CRT591_BaseResponseMessage();
        }
#endregion

        CRT591_PositiveResponseMessage DecodePositiveResponse(CRT591_BaseResponseMessage BaseOfMessage, byte LENH, byte LENL, byte[] Message)
        {
            //ST0 CardStatus
            CRT591_CardStatus CardStatus = (CRT591_CardStatus)Message[7];
            //ST1 CardStack 
            CRT591_CardStackStatus StackStatus = (CRT591_CardStackStatus)Message[8];
            //ST2 Error bin status
            CTR591_ErrorCardBinStatus ErrorBinStatus = (CTR591_ErrorCardBinStatus)Message[9];

            byte[] Data = new byte[LENL - 6];
            Array.Copy(Message, 10, Data, 0, Data.Length);

            return new CRT591_PositiveResponseMessage(BaseOfMessage.MachineAddress, BaseOfMessage.Command, BaseOfMessage.Param, CardStatus, StackStatus, ErrorBinStatus, Data);
        }

        CRT591_NegativeResponseMessage DecodeNegativeResponse(CRT591_BaseResponseMessage BaseOfMessage, byte LENH, byte LENL, byte[] Message)
        {
            string ErrorString = "";
            ErrorString += (char)Message[7];
            ErrorString += (char)Message[8];
            CTR591_Errors Error = GetError(ErrorString);

            byte[] Data = new byte[LENL];
            Array.Copy(Message, 9, Data, 0, Data.Length);

            return new CRT591_NegativeResponseMessage(BaseOfMessage.MachineAddress, BaseOfMessage.Command, BaseOfMessage.Param, Error, Data);
        }
#endregion

#region helpers
        CRT591_CardStatus GetCardStatus(byte In)
        {
            switch (In)
            {
                case 0x30:
                    return CRT591_CardStatus.CardStatus_NoCard;
                case 0x31:
                    return CRT591_CardStatus.CardStatus_CardInGate;
                case 0x32:
                    return CRT591_CardStatus.CardStatus_CardInRFPostion;
                default:
                    return CRT591_CardStatus.CardStatus_Unkown;
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

#region Utilities
        CTR591_Errors GetError(string StrError)
        {
            switch (StrError)
            {
                case "00":
                    return CTR591_Errors.Error_CommandUndefined;
                case "01":
                    return CTR591_Errors.Error_CommandParameterError;
                case "02":
                    return CTR591_Errors.Error_CommandSquenceError;
                case "03":
                    return CTR591_Errors.Error_CommandNotSupportedByHardware;
                case "04":
                    return CTR591_Errors.Error_CommandDataError;
                case "05":
                    return CTR591_Errors.Error_CardContactIssue;
                case "10":
                    return CTR591_Errors.Error_CardJam;
                case "12":
                    return CTR591_Errors.Error_SensorError;
                case "13":
                    return CTR591_Errors.Error_CardTooLong;
                case "14":
                    return CTR591_Errors.Error_CardTooShort;
                case "40":
                    return CTR591_Errors.Error_CardRecyclingDisabled;
                case "41":
                    return CTR591_Errors.Error_CardMagneticRailError;
                case "43":
                    return CTR591_Errors.Error_CardPostionMoveDisabled;
                case "45":
                    return CTR591_Errors.Error_CardManuallyMove;
                case "50":
                    return CTR591_Errors.Error_CardCounterOverflow;
                case "51":
                    return CTR591_Errors.Error_MotorError;
                case "60":
                    return CTR591_Errors.Error_CardPowerSupplyShort;
                case "61":
                    return CTR591_Errors.Error_CardActiviationFailure;
                case "62":
                    return CTR591_Errors.Error_ICCommandNotSupportedByCard;
                case "65":
                    return CTR591_Errors.Error_ICCardDisabled;
                case "66":
                    return CTR591_Errors.Error_ICCommandNotSupportedByCardAtThisTime;
                case "67":
                    return CTR591_Errors.Error_ICCardTransmittionError;
                case "68":
                    return CTR591_Errors.Error_ICCardTransmittionOvertime;
                case "69":
                    return CTR591_Errors.Error_CPUSAMNonEMVStandardCompliance;
                case "A0":
                    return CTR591_Errors.Error_EmptyStacker;
                case "A1":
                    return CTR591_Errors.Error_ErrorCardBinFull;
                case "B0":
                    return CTR591_Errors.Error_NotMapped;
            }
            return CTR591_Errors.Error_NotMapped;
        }
#endregion
    }
}
