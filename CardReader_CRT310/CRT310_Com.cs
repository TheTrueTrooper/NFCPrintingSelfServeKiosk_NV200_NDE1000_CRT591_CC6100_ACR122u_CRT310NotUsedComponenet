using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace CardReader_CRT310
{
    /// <summary>
    /// A com Port Protocol for the card reader (Credit/Debit)
    /// </summary>
    public class CRT310_Com : IDisposable
    {
        enum Commands
        {
            CardStopPositionSetup = 0xE2,
            CardInControl = 0x2F,
            Reset = 0x30,
            ReadWriteSN = 0x30,
            CheckStatus = 0x31,
            CheckICCardType = 0x31,
            MoveCardOperation = 0x32,
            ICCardPowerOnOff = 0x33,
            SIMCardPowerOff = 0x4A,
            SetCommBaudRate = 0x34,
        }

        /// <summary>
        /// Gets if the cardreader has been Initialized
        /// </summary>
        public bool Initialized { private set; get; } = false;


        const string InitError = "Reader has not been Initialized. Please call 'SendResetInitCommand' to Initialize before use.";
        const string DisposedError = "Reader has already been disposed and marked for clean up.";

        /// <summary>
        /// Start of Text Message
        /// </summary>
#if DEBUG
        public const byte STX = 0x02;
#else
        const byte STX = 0x02;
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
        /// Positive acknologment of command
        /// </summary>
#if DEBUG
        public const byte ENQ = 0x05;
#else
        const byte ENQ = 0x05;
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
        /// Gets if this has been disposed
        /// </summary>
        public bool Disposed { get; private set; } = false;

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

        /// <summary>
        /// Creates a reader and its commands
        /// </summary>
        /// <param name="SerialPortName"></param>
        public CRT310_Com(string SerialPortName)
        {
            SerialPort = new SerialPort(SerialPortName, BaudRate, Parity.None, DataSize, StopBits.One);
        }

        /// <summary>
        /// opens the port
        /// </summary>
        public void OpenCom()
        {
            SerialPort.Open();
        }

        #region Commands
        #region BaseSendCommand
        /// <summary>
        /// Sends an internal command nicely framed. (not required for you)
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="SubCommand"></param>
        /// <param name="CommandData"></param>
        /// <returns></returns>
        CRT310_PositiveResponseMessage SendCommand(byte Command, byte SubCommand, byte[] CommandData)
        {
            //the Message XOR check (XOR all all bytes and check against the last byte) 
            byte XORCheck = 0;
            //the High byte start will alway be 0
            byte LENH = 0;
            //this will aways be the length of the extra attached data plus the 2 always included data for the command
            byte LENL = (byte)(CommandData.Length + 2);
            //this will all ways be the data plus 6 for frame data
            int MessageLength = LENL + 5;

            byte[] Message = new byte[MessageLength];

            //set the start of the frame
            Message[0] = STX;
            Message[1] = LENH;
            Message[2] = LENL;
            Message[3] = Command;
            Message[4] = SubCommand;
            //load all the variable data
            Array.Copy(CommandData, 0, Message, 5, CommandData.Length);
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

            SerialPort.Write(new byte[] { ENQ }, 0, 1);

            List<byte> Response = new List<byte>();
            byte Char;
            do
            {
                Char = (byte)SerialPort.ReadByte();
                Response.Add(Char);
            }
            while (Response.Count < 4 || Response[Response.Count - 1] != ETX);

            Char = (byte)SerialPort.ReadByte();
            Response.Add(Char);

            CRT310_BaseResponseMessage Return = DecodeResponse(Response.ToArray());

            if (Return is CRT310_NegativeResponseMessage)
                throw new CRT310_CommandException((CRT310_NegativeResponseMessage)Return);

            //send acknologment
            SerialPort.Write(new byte[] { ACK }, 0, 1);
            return (CRT310_PositiveResponseMessage)Return;
        }
        #endregion

        /// <summary>
        /// The init command to set up the reader
        /// </summary>
        /// <param name="InItParam">The action to perform with any left over cards</param>
        /// <returns>The string with the reader and vers number</returns>
        public string ResetInitCommand(CRT310_Commands_InitParam InItParam = CRT310_Commands_InitParam.ResetAndReturnVersion)
        {
            if (Disposed)
                throw new Exception(DisposedError);

            string RevType;
            byte Command = (byte)Commands.Reset;
            byte CommandParameter = (byte)InItParam;
            byte[] CommandData = new byte[0];

            CRT310_PositiveResponseMessage Return = SendCommand(Command, CommandParameter, CommandData);
            RevType = ASCIIEncoding.ASCII.GetString(Return.DataRaw);

            Initialized = true;

            return RevType;
        }

        /// <summary>
        /// Get the status of the card reader (includes if card is inside) for polling
        /// </summary>
        /// <returns>a pakage with if there is a card inside and the status of the gates</returns>
        public CRT310_ReaderStatus ReaderStatus()
        {
            if (!Initialized)
                throw new Exception(InitError);
            if (Disposed)
                throw new Exception(DisposedError);
            byte Command = (byte)Commands.CheckStatus;
            byte CommandParameter = (byte)CRT310_Commands_CardReaderStatusParam.ReaderStatus;
            byte[] CommandData = new byte[0];

            CRT310_PositiveResponseMessage Return = SendCommand(Command, CommandParameter, CommandData);

            return new CRT310_ReaderStatus((CRT310_CardStatus)Return.DataRaw[0], (CRT310_CardReaderFrontStatus)Return.DataRaw[1], (CRT310_CardReaderRearStatus)Return.DataRaw[2]);
        }

        /// <summary>
        /// Get the status of the card reader for polling
        /// </summary>
        /// <returns>A package with all the readers sensors</returns>
        public CRT310_SensorStatuss SensorStatus()
        {
            if (!Initialized)
                throw new Exception(InitError);
            if (Disposed)
                throw new Exception(DisposedError);
            byte Command = (byte)Commands.CheckStatus;
            byte CommandParameter = (byte)CRT310_Commands_CardReaderStatusParam.SensorStatus;
            byte[] CommandData = new byte[0];

            CRT310_PositiveResponseMessage Return = SendCommand(Command, CommandParameter, CommandData);

            CRT310_SensorStatus[] Sensors = new CRT310_SensorStatus[5];
            Sensors[0] = (CRT310_SensorStatus)Return.DataRaw[0];
            Sensors[1] = (CRT310_SensorStatus)Return.DataRaw[1];
            Sensors[2] = (CRT310_SensorStatus)Return.DataRaw[2];
            Sensors[3] = (CRT310_SensorStatus)Return.DataRaw[3];
            Sensors[4] = (CRT310_SensorStatus)Return.DataRaw[4];

            return new CRT310_SensorStatuss(Sensors, (CRT310_ShutterStatus)Return.DataRaw[5], (CRT310_SwitchStatus)Return.DataRaw[6]);
        }

        /// <summary>
        /// Moves the card to any of the postions or to eject (Front or back)
        /// </summary>
        /// <param name="Param">The postion to move to</param>
        /// <returns>the status of the operation</returns>
        public CRT310_CardOperationStatus MoveCard(CRT310_Commands_MoveParam Param = CRT310_Commands_MoveParam.EjectCardFront)
        {
            if (!Initialized)
                throw new Exception(InitError);
            if(Disposed)
                throw new Exception(DisposedError);
            byte Command = (byte)Commands.MoveCardOperation;
            byte CommandParameter = (byte)Param;
            byte[] CommandData = new byte[0];

            CRT310_PositiveResponseMessage Return = SendCommand(Command, CommandParameter, CommandData);

            return (CRT310_CardOperationStatus)Return.DataRaw[0];
        }
        #endregion

        //Not important to you
        #region MessageDecoding
        #region BaseMessageDecode
        CRT310_BaseResponseMessage DecodeResponse(byte[] Message)
        {
            //byte XORCheck = 0;
            //for (byte i = 0; i < Message.Length; i++)
            //    XORCheck ^= i;
            //XORCheck

            // Negative message Command Header
            const byte CHN = 0x4E;

            //build a set of varables we can set
            CRT310_MessageResponseStatus MessageStatus = CRT310_MessageResponseStatus.UnkownFormateAssumedNotFor;

            byte LENH = 0x00;
            byte LENL = 0x03;

            //if the message is not a message return unknown
            if (Message[0] != STX)
                return new CRT310_BaseResponseMessage(CRT310_MessageResponseStatus.UnkownFormateAssumedNotFor);

            //fill i the basics
            LENH = Message[1];
            LENL = Message[2];

            if (Message[3] == CHN)
                MessageStatus = CRT310_MessageResponseStatus.Negative;
            else
                MessageStatus = CRT310_MessageResponseStatus.Positive;


            CRT310_BaseResponseMessage Return = new CRT310_BaseResponseMessage(MessageStatus);

            if (MessageStatus == CRT310_MessageResponseStatus.Positive)
                Return = DecodePositiveResponse(Return, LENH, LENL, Message);
            else if (MessageStatus == CRT310_MessageResponseStatus.Negative)
                Return = DecodeNegativeResponse(Return, LENH, LENL, Message);

            return Return;
        }
        #endregion

        CRT310_PositiveResponseMessage DecodePositiveResponse(CRT310_BaseResponseMessage BaseOfMessage, byte LENH, byte LENL, byte[] Message)
        {
            byte Command = Message[3];
            byte CommandParam = Message[4];

            byte[] Data = new byte[LENL - 2];
            Array.Copy(Message, 5, Data, 0, Data.Length);

            return new CRT310_PositiveResponseMessage(BaseOfMessage.Command, CommandParam, Data);
        }

        CRT310_NegativeResponseMessage DecodeNegativeResponse(CRT310_BaseResponseMessage BaseOfMessage, byte LENH, byte LENL, byte[] Message)
        {
            byte Command = Message[4];
            CRT310_Errors Error = (CRT310_Errors)Message[5];

            byte[] Data = new byte[LENL - 3];
            Array.Copy(Message, 6, Data, 0, Data.Length);

            return new CRT310_NegativeResponseMessage(Command, Error, Data);
        }
        #endregion

        /// <summary>
        /// standard dispose
        /// </summary>
        public void Dispose()
        {
            Disposed = true;
            SerialPort.Dispose();
        }

        /// <summary>
        /// A deconstructor to ensure resorces are freed.
        /// </summary>
        ~CRT310_Com()
        {
            if(!Disposed)
                SerialPort?.Dispose();
        }
    }
}
