using CoinChanger_MDBRS232_For_CC6100.MDBEnums;
using CoinChanger_MDBRS232_For_CC6100.MDBEnums.CAD;
using CoinChanger_MDBRS232_For_CC6100.MDBEnums.CoinChangerEnums.CoinChangerEnums;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinChanger_MDBRS232_For_CC6100
{
    public class CC6100MDB_Com : IDisposable
    {
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
        /// poll rate 150 – 300 ms
        /// Time IU waits for the Master’s poll before inhibiting acceptance of the BA and CC(max) 3 second
        /// </summary>
        /// <param name="SerialPortName"></param>
        public CC6100MDB_Com(string SerialPortName)
        {
            SerialPort = new SerialPort(SerialPortName, BaudRate, Parity.None, DataSize, StopBits.One);
            SerialPort.ReceivedBytesThreshold = 1;
            SerialPort.DataReceived += ReceivedData;
        }

        private void ReceivedData(object sender, SerialDataReceivedEventArgs e)
        {
            byte FirstByte = (byte)SerialPort.ReadByte();

            if (FirstByte == (byte)CC6100MDB_Responses.ACK)
                return;

            if (FirstByte == (byte)CC6100MDB_Responses.NACK)
                throw new Exception("Error!");

            //switch ((CC6100MDB_DeviceAddresses)FirstByte)
            //{
            //    case CC6100MDB_DeviceAddresses.CoinChanger:
            //        if ()
            //            return;
            //}

        }

        /// <summary>
        /// opens the port
        /// </summary>
        public void OpenCom()
        {
            SerialPort.Open();
        }

        /// <summary>
        /// Paysoutcoin
        /// </summary>
        /// <param name="Amount">the single coin that you wish to pay out</param>
        /// <returns></returns>
        public void PayoutCoin(CC6100MDB_CADScalingFactors Amount)
        {
            byte[] Command = new byte[3]; //new byte[4];
            Command[0] = (byte)CC6100MDB_Commands.CCExpansionCommand;
            Command[1] = (byte)CC6100MDB_CCExpansionCommands.Payout;
            Command[2] = (byte)Amount;
            //Command[3] = (byte)(Command[1] + Command[2]); when working with the test doc the check sum doesnt seem to work. May have been dropped and docs may be old
            //Command[3] = 0xFF;
            SerialPort.Write(Command, 0, Command.Length);
        }

        /// <summary>
        /// Paysoutcoin
        /// </summary>
        /// <param name="Amount">the total amount that you wish to pay out</param>
        /// <returns></returns>
        public void PayoutCoin(byte Amount)
        {
            byte[] Command = new byte[3]; //new byte[4];
            Command[0] = (byte)CC6100MDB_Commands.CCExpansionCommand;
            Command[1] = (byte)CC6100MDB_CCExpansionCommands.Payout;
            Command[2] = Amount;
            //Command[3] = (byte)(Command[1] + Command[2]); when working with the test doc the check sum doesnt seem to work. May have been dropped and docs may be old
            //Command[3] = 0xFF;
            SerialPort.Write(Command, 0, Command.Length);
        }

        public void Dispose()
        {
            if(!Disposed)
                SerialPort.Dispose();
        }

        ~CC6100MDB_Com()
        {
            if (!Disposed)
                SerialPort.Dispose();
        }
    }
}
