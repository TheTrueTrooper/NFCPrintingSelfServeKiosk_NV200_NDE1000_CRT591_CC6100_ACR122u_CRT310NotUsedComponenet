using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace CardReader_CRT_310
{
    public class CRT310_Com : IDisposable
    {
        enum Commands
        {
            INITIALIZE = 0x30,
            STATUSREQUEST = 0x31,
            CARDMOVE = 0x32,
            CARDENTRY = 0x33,
            MagneticCardOperation = 0x35,
            CombineMagneticCardOperation = 0x36,
            ICRFIDCardType = 0x50, //<------------
            CPUCARDCONTROL = 0x51,
            SAMCARDCONTROL = 0x52,
            SLE4442_4428CARDCONTROL = 0x53,
            I2CMEMORYCARD = 0x54,
            RFIDCARDCONTROL = 0x60,
            LEDIndicatorOperation = 0x80,
            PartsMaintenancetLifeTimeParam = 0xA1,
            MACHINESERIALNUMBER = 0xA2,
            READMACHINECONFIG = 0xA3,
            READCRT310VERSION = 0xA4,
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


        public CRT310_Com(string SerialPortName, byte MachinesAddress = 0)
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
        CRT310_PositiveResponseMessage SendCommand(byte Command, byte SubCommand, byte[] CommandData)
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
            CRT310_BaseResponseMessage Return = DecodeResponse(Response.ToArray());
            if (Return is CRT310_NegativeResponseMessage)
                throw new CRT310_CommandException((CRT310_NegativeResponseMessage)Return);
            return (CRT310_PositiveResponseMessage)Return;
        }
        #endregion

        public string SendResetInitCommand(CRT310_Commands_InitParam InItParam = CRT310_Commands_InitParam.DoNotMove)
        {
            string RevType;
            byte command = (byte)Commands.INITIALIZE;
            byte commandParameter = (byte)InItParam;
            byte[] commandData = new byte[0];

            CRT310_PositiveResponseMessage Return = SendCommand(command, commandParameter, commandData);
            RevType = ASCIIEncoding.ASCII.GetString(Return.DataRaw);

            Initialized = true;

            return RevType;
        }

        public void MoveCardCommand(CRT310_Commands_MoveCardParam MoveCardParma = CRT310_Commands_MoveCardParam.MoveCardToFrontSideWithoutCardHolding)
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.CARDMOVE;
            byte commandParameter = (byte)MoveCardParma;
            byte[] commandData = new byte[0];

            SendCommand(command, commandParameter, commandData);
        }

        public void CardGateEntrySetCommand(CRT310_Commands_SetCardEntryParam GateSetting = CRT310_Commands_SetCardEntryParam.ProhibitEntryFromFront)
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.CARDENTRY;
            byte commandParameter = (byte)GateSetting; //Disable card input from gate
            byte[] commandData = new byte[0];

            SendCommand(command, commandParameter, commandData);
        }

        public CRT310_SensorStatus[] RequestStatus(CRT310_Commands_GetStatusParam StatusParma = CRT310_Commands_GetStatusParam.GetCRT591Status)
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.STATUSREQUEST;
            byte commandParameter = (byte)StatusParma; //Disable card input from gate
            byte[] commandData = new byte[0];
            CRT310_PositiveResponseMessage Return = SendCommand(command, commandParameter, commandData);
            CRT310_SensorStatus[] Sensors = new CRT310_SensorStatus[Return.DataRaw.Length];
            for (int i = 0; i < Return.DataRaw.Length; i++)
                Sensors[i] = (CRT310_SensorStatus)Return.DataRaw[i];
            return Sensors;
        }

        public int ReadBinCounter()
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.RECYCLEBINCOUNTER;
            byte commandParameter = (byte)CRT310_Commands_RecycleBinCounterParam.ReadCardCounter; //Disable card input from gate
            byte[] commandData = new byte[0];
            byte[] CountBytes = new byte[4];

            CRT310_PositiveResponseMessage Return = SendCommand(command, commandParameter, commandData);

            if (BitConverter.IsLittleEndian)
                Array.Copy(Return.DataRaw, CountBytes, Return.DataRaw.Length);
            else
                Array.Copy(Return.DataRaw, 1, CountBytes, 0, Return.DataRaw.Length);

            return BitConverter.ToInt32(CountBytes, 0);
        }

        public void SetInitBinCounter(int Counter)
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.RECYCLEBINCOUNTER;
            byte commandParameter = (byte)CRT310_Commands_RecycleBinCounterParam.InitiateCardCounter; //Disable card input from gate
            byte[] commandData = new byte[3];
            byte[] Int32Bytes = BitConverter.GetBytes(Counter);

            if (BitConverter.IsLittleEndian)
                Array.Copy(Int32Bytes, 0, commandData, 0, commandData.Length);
            else
                Array.Copy(Int32Bytes, 1, commandData, 0, commandData.Length);

            SendCommand(command, commandParameter, commandData);
        }

        public string ReadCRT591FirmwareVers()
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.READCRT310VERSION;
            byte commandParameter = 0x30; //Disable card input from gate
            byte[] commandData = new byte[0];

            CRT310_PositiveResponseMessage Return = SendCommand(command, commandParameter, commandData);
            return ASCIIEncoding.ASCII.GetString(Return.DataRaw);
        }

        public string ReadCardConfig()
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.READMACHINECONFIG;
            byte commandParameter = 0x30; //Disable card input from gate
            byte[] commandData = new byte[0];

            CRT310_PositiveResponseMessage Return = SendCommand(command, commandParameter, commandData);
            return ASCIIEncoding.ASCII.GetString(Return.DataRaw);
        }

        public byte[] ReadCardSerialNumber()
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.MACHINESERIALNUMBER;
            byte commandParameter = 0x30; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData).DataRaw;
        }

        public CRT310_PositiveResponseMessage ReadCardType(CRT310_Commands_CheckTypeRForICParam CheckTypeParam = CRT310_Commands_CheckTypeRForICParam.ICCardType)
        {
            if (!Initialized)
                throw new Exception(InitError);

            byte command = (byte)Commands.ICRFIDCardType;
            byte commandParameter = (byte)CheckTypeParam; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        #region SendCommandsForSpecificCards 
        internal CRT310_PositiveResponseMessage SendRFCardControl(CRT310_Commands_MifareRFOperationParam CardOperation = CRT310_Commands_MifareRFOperationParam.Startup)
        {
            byte command = (byte)Commands.RFIDCARDCONTROL;
            byte commandParameter = (byte)CardOperation; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        internal CRT310_PositiveResponseMessage SendCPUCardControl(CRT310_Commands_CPUOperationParam CardOperation = CRT310_Commands_CPUOperationParam.ColdReset)
        {
            byte command = (byte)Commands.CPUCARDCONTROL;
            byte commandParameter = (byte)CardOperation; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        internal CRT310_PositiveResponseMessage SendSAMCardControl(CRT310_Commands_SAMOperationParam CardOperation = CRT310_Commands_SAMOperationParam.ColdReset)
        {
            byte command = (byte)Commands.SAMCARDCONTROL;
            byte commandParameter = (byte)CardOperation; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        internal CRT310_PositiveResponseMessage SendICCardControl(CRT310_Commands_SAMOperationParam CardOperation = CRT310_Commands_SAMOperationParam.ColdReset)
        {
            byte command = (byte)Commands.I2CMEMORYCARD;
            byte commandParameter = (byte)CardOperation; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }

        internal CRT310_PositiveResponseMessage SendSLE4442_4428CardControl(CRT310_Commands_SAMOperationParam CardOperation = CRT310_Commands_SAMOperationParam.ColdReset)
        {
            byte command = (byte)Commands.SLE4442_4428CARDCONTROL;
            byte commandParameter = (byte)CardOperation; //Disable card input from gate
            byte[] commandData = new byte[0];

            return SendCommand(command, commandParameter, commandData);
        }
        #endregion
        #endregion

        #region MessageDecoding
        #region BaseMessageDecode
        CRT310_BaseResponseMessage DecodeResponse(byte[] Message)
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
            CRT310_MessageResponseStatus MessageStatus = CRT310_MessageResponseStatus.UnkownFormateAssumedNotFor;
            byte Command = 0x00;
            byte Param = 0x00;

            byte LENH = 0x00;
            byte LENL = 0x03;

            byte MessageAddress = 0x00;

            //if the message is not a message return unknown
            if (Message[0] != STX)
                return new CRT310_BaseResponseMessage(CRT310_MessageResponseStatus.UnkownFormateAssumedNotFor);
            //if the message is not for the machine return wrong target
            if (Message[1] != MachinesAddress)
                return new CRT310_BaseResponseMessage(CRT310_MessageResponseStatus.NotForThisInstance);

            //fill i the basics
            MessageAddress = Message[1];
            LENH = Message[2];
            LENL = Message[3];

            if (Message[4] == CHP)
                MessageStatus = CRT310_MessageResponseStatus.Positive;
            else if (Message[4] == CHN)
                MessageStatus = CRT310_MessageResponseStatus.Negative;

            Command = Message[5];
            Param = Message[6];

            CRT310_BaseResponseMessage Return = new CRT310_BaseResponseMessage(MessageStatus, MachinesAddress, Command, Param);

            if (MessageStatus == CRT310_MessageResponseStatus.Positive)
                Return = DecodePositiveResponse(Return, LENH, LENL, Message);
            else if (MessageStatus == CRT310_MessageResponseStatus.Negative)
                Return = DecodeNegativeResponse(Return, LENH, LENL, Message);

            return Return;
        }
        #endregion

        CRT310_PositiveResponseMessage DecodePositiveResponse(CRT310_BaseResponseMessage BaseOfMessage, byte LENH, byte LENL, byte[] Message)
        {
            //ST0 CardStatus & ST1 CardStack  but it doesnt go above 10 so lets cheat
            CRT310_CardStatus CardStatus = (CRT310_CardStatus)Message[8];
            //ST1 CardStack 
 

            byte[] Data = new byte[LENL - 6];
            Array.Copy(Message, 9, Data, 0, Data.Length);

            return new CRT310_PositiveResponseMessage(BaseOfMessage.MachineAddress, BaseOfMessage.Command, BaseOfMessage.Param, CardStatus, Data);
        }

        CRT310_NegativeResponseMessage DecodeNegativeResponse(CRT310_BaseResponseMessage BaseOfMessage, byte LENH, byte LENL, byte[] Message)
        {
            string ErrorString = "";
            ErrorString += (char)Message[7];
            ErrorString += (char)Message[8];
            CRT310_Errors Error = GetError(ErrorString);

            byte[] Data = new byte[LENL];
            Array.Copy(Message, 9, Data, 0, Data.Length);

            return new CRT310_NegativeResponseMessage(BaseOfMessage.MachineAddress, BaseOfMessage.Command, BaseOfMessage.Param, Error, Data);
        }
        #endregion

        #region helpers
        CRT310_CardStatus GetCardStatus(byte In)
        {
            switch (In)
            {
                case 0x30:
                    return CRT310_CardStatus.CardStatus_NoCard;
                case 0x31:
                    return CRT310_CardStatus.CardStatus_CardInGate;
                case 0x32:
                    return CRT310_CardStatus.CardStatus_CardInRFPostion;
                default:
                    return CRT310_CardStatus.CardStatus_Unkown;
            }
        }
        #endregion

        public void Dispose()
        {
            SerialPort?.Dispose();
        }

        ~CRT310_Com()
        {
            SerialPort?.Dispose();
        }

        #region Utilities
        CRT310_Errors GetError(string StrError)
        {
            switch (StrError)
            {
                case "00":
                return CRT310_Errors.Error_CommandCharacterError;
                case "01":
                    return CRT310_Errors.Error_CommandParameterError;
                case "02":
                    return CRT310_Errors.Error_CommandCanNotBeExecuted;
                case "03":
                    return CRT310_Errors.Error_OutOfHardwareSupportCommand;
                case "04":
                    return CRT310_Errors.Error_CommandDataError;
                case "10":
                    return CRT310_Errors.Error_CardJam;
                case "11":
                    return CRT310_Errors.Error_Shuttererror;
                case "13":
                    return CRT310_Errors.Error_CardTooLong;
                case "14":
                    return CRT310_Errors.Error_CardTooShort;
                case "15":
                    return CRT310_Errors.Error_EEPROMError;
                case "16":
                    return CRT310_Errors.Error_CardPulledOutByForce;
                case "17":
                    return CRT310_Errors.Error_CardJamWhenInsert;
                case "19":
                    return CRT310_Errors.Error_CardNotInsertFromRear;
                case "20":
                    return CRT310_Errors.Error_ReadError_CRCError;
                case "21":
                    return CRT310_Errors.Error_ReadError;
                case "23":
                    return CRT310_Errors.Error_ReadError_OnlySS_ES_LRC;
                case "24":
                    return CRT310_Errors.Error_ReadError_NoDataBlank;
                case "26":
                    return CRT310_Errors.Error_ReadErrorNoSS;
                case "27":
                    return CRT310_Errors.Error_ReadErrorNoES;
                case "28":
                    return CRT310_Errors.Error_ReadErrorLRCError;
                case "30":
                    return CRT310_Errors.Error_PowerDown;
                case "32":
                    return CRT310_Errors.Error_VoltageTooHigh;
                case "33":
                    return CRT310_Errors.Error_VoltageTooLow;
                case "40":
                    return CRT310_Errors.Error_CardPulledWhenRetreating;
                case "41":
                    return CRT310_Errors.Error_ICCardOperationError;
                case "43":
                    return CRT310_Errors.Error_DisableToMoveCardToICCardPosition;
                case "45":
                    return CRT310_Errors.Error_CardWithdrawError;
                case "50":
                    return CRT310_Errors.Error_CardCounterOverflow;
                case "60":
                    return CRT310_Errors.Error_PowerShortOnICCard;
                case "61":
                    return CRT310_Errors.Error_ATRError;
                case "62":
                    return CRT310_Errors.Error_ICCardTypeError;
                case "63":
                    return CRT310_Errors.Error_ICCardDisabled;
                case "64":
                    return CRT310_Errors.Error_Otherthan63;
                case "65":
                    return CRT310_Errors.Error_SendCPUCommandBeforeATR;
                case "66":
                    return CRT310_Errors.Error_CommandOutOfICCurrentCardSupport;
                case "69":
                    return CRT310_Errors.Error_ICCardNonComplianceToEMVStandard;
                case "90":
                    return CRT310_Errors.Error_UnknownCardType;
                case "B0":
                    return CRT310_Errors.Error_NotReceiveResetCommand; //B0
            }
            return CRT310_Errors.Error_NotMapped;
        }
        #endregion
    }
}
