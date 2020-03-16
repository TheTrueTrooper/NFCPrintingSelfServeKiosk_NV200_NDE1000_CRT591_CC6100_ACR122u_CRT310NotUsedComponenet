using System;
using System.Text;
using System.IO.Ports;

namespace BillDispenser_NDE1000
{
    public class NDE1000_Com
    {
        enum Commands : byte
        {
            DispenseNotes = (byte)'B',
            ClearData = (byte)'I',
            DisableEnableSwitches = (byte)'K',
            GetStatus = (byte)'S',
            GetAccumulatedAmountDispensed = (byte)'C',
            SetRealTimeClock = (byte)'T',
            GetRealTimeClock = (byte)'R',
        }

        const int BuadRate = 9600;

        const int Data = 8;

        const byte STX = 0x02;
        const byte ETX = 0x03;

        const byte ACK = 0x06;
        const byte NCK = 0x0A;

        public byte Address { get; private set; }

        SerialPort ComPort;

        public NDE1000_Com(string ComPort, byte Address = 0)
        {
            this.Address = Address;

            this.ComPort = new SerialPort(ComPort, BuadRate, Parity.None, Data, StopBits.One);
        }

        public void OpenCom()
        {
            ComPort.Open();
        }

        byte[] SendCommand(byte Command, byte[] Data = null)
        {
            if (Data == null)
                Data = new byte[] { 0x30, 0x30, 0x30, 0x30 };
            const int MessageSize = 10;
            byte CS = 0;
            byte[] Message = new byte[MessageSize];
            Message[0] = STX;
            string ID = String.Format("{0,18:00}", Address);

            Message[1] = (byte)ID[0];
            Message[2] = (byte)ID[1];
            Message[3] = Command;
            Message[4] = Data[0];
            Message[5] = Data[1];
            Message[6] = Data[2];
            Message[7] = Data[3];

            for(byte i = 0; i < 8; i++)
            {
                CS ^= Message[i];
            }

            Message[8] = CS;
            Message[9] = ETX;

            ComPort.Write(Message, 0, MessageSize);

            byte Answer = (byte)ComPort.ReadByte();
            switch(Answer)
            {
                case ACK:
                    return null;
                case NCK:
                    throw new Exception("Negative response was returned");
                case STX:
                    byte[] Return = new byte[10];
                    Return[0] = Answer;
                    for(int i = 1; i < Return.Length; i++)
                        Return[i] = (byte)ComPort.ReadByte();
                    return Return;
            }
            throw new Exception("Error in recived response. known message recived");
        }

        /// <summary>
        /// simply despenses the amount of notes desired and returns the actuall dispensed amount,
        /// </summary>
        /// <param name="Amount"></param>
        /// <returns></returns>
        public int DispenseNotes(int Amount)
        {
            string StAmount = String.Format("{0,18:000}", Amount);
            //byte[] Answer = SendCommand((byte)Commands.DispenseNotes, new byte[] { 0x30, (byte)StAmount[0], (byte)StAmount[1], (byte)StAmount[2] });
            SendCommand((byte)Commands.DispenseNotes, new byte[] { 0x30, (byte)StAmount[0], (byte)StAmount[1], (byte)StAmount[2] });
            byte[] Answer = new byte[10];
            for (int i = 0; i < Answer.Length; i++)
                Answer[i] = (byte)ComPort.ReadByte();
            return int.Parse(ASCIIEncoding.ASCII.GetString(Answer, 5, 3));
        }

        /// <summary>
        /// clears a data register
        /// </summary>
        /// <param name="DataToClear"></param>
        public void ClearData(NDE1000_ClearData DataToClear)
        {
            SendCommand((byte)Commands.ClearData, new byte[] { 0x30, 0x30, 0x30, (byte)DataToClear });
        }

        /// <summary>
        /// Locks or unlocks keys
        /// </summary>
        /// <param name="LockStartKey">true for lock</param>
        /// <param name="LockClearKey">true for lock</param>
        public void LockButtons(bool LockStartKey = true, bool LockClearKey = true)
        {
            byte[] Data = new byte[] { 0x30, 0x30, 0x30, 0x30 };
            if (LockStartKey)
                Data[0] = 0x31;
            if (LockClearKey)
                Data[2] = 0x31;
            SendCommand((byte)Commands.DisableEnableSwitches, Data);
        }

        /// <summary>
        /// Gets the machines oveall status
        /// </summary>
        /// <returns></returns>
        public NDE1000_StatusReturn GetStatus()
        {
            //byte[] Answer = SendCommand((byte)Commands.DispenseNotes);
            SendCommand((byte)Commands.GetStatus);
            byte[] Answer = new byte[10];
            for (int i = 0; i < Answer.Length; i++)
                Answer[i] = (byte)ComPort.ReadByte();

            return new NDE1000_StatusReturn((NDE1000_Status)Answer[4], (NDE1000_Errors)Answer[5], (NDE1000_KeySettings)Answer[6], (NDE1000_KeySettings)Answer[7]);

        }

        /// <summary>
        /// Gets the status of the last dispensal
        /// </summary>
        /// <returns></returns>
        public NDE1000_DispensalCheckReturn CheckDispensalStatus()
        {
            //byte[] Answer = SendCommand((byte)Commands.DispenseNotes);
            SendCommand((byte)Commands.GetStatus);
            byte[] Answer = new byte[10];
            for (int i = 0; i < Answer.Length; i++)
                Answer[i] = (byte)ComPort.ReadByte();

            return new NDE1000_DispensalCheckReturn(int.Parse(ASCIIEncoding.ASCII.GetString(Answer, 5, 3)), (NDE1000_Errors)Answer[4]);

        }

        public void WriteRealTimeClock(byte Year, byte Month, byte Day, byte Hour, byte Minute, byte Second)
        {
            SendCommand((byte)Commands.SetRealTimeClock, new byte[] { (byte)'d', Year, Month, Day });
            SendCommand((byte)Commands.SetRealTimeClock, new byte[] { (byte)'t', Hour, Minute, Second });
        }

        public DateTime GetRealTimeClock()
        {
            //byte[] Answer = SendCommand((byte)Commands.DispenseNotes);
            SendCommand((byte)Commands.GetRealTimeClock, new byte[] { (byte)'d', 0x30, 0x30, 0x30 });
            byte[] Answer = new byte[10];
            for (int i = 0; i < Answer.Length; i++)
                Answer[i] = (byte)ComPort.ReadByte();
            byte Year = Answer[5];
            byte Month = Answer[6];
            byte Day = Answer[7];
            SendCommand((byte)Commands.GetRealTimeClock, new byte[] { (byte)'t', 0x30, 0x30, 0x30 });
            Answer = new byte[10];
            for (int i = 0; i < Answer.Length; i++)
                Answer[i] = (byte)ComPort.ReadByte();
            byte Hour = Answer[5];
            byte Minute = Answer[5];
            byte Second = Answer[5];
            return new DateTime(Year, Month, Day, Hour, Minute, Second);
        }

    }
}
