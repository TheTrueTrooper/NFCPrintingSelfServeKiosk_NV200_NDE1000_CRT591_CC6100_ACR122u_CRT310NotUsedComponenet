using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITLlib;

namespace BillValidator_NV200
{
    public class NV200_Com
    {
        const string CommandHasFailedError = "The {0} Command has failed with code {1}.";

        const byte ProtocolVers = 0x07;

        SSPComms ComManager;
        SSP_COMMAND CommandFrame;
        SSP_KEYS keys;
        SSP_FULL_KEY sspKey;
        SSP_COMMAND_INFO CInfo;

        ///// <summary>
        ///// Returns the Serial Ports Name
        ///// </summary>
        //string PortName
        //{
        //    get
        //    {
        //        return SerialPort?.PortName;
        //    }
        //}

        List<NV200_ChannelData> Channels;

        const char UnitTypeCode = (char)0x00;

        /// <summary>
        /// Creates a com port.
        /// note you will still need to call 'OpenCom' to open the port
        /// and then 'ResetInitCommand' to reset the machine befor use 
        /// </summary>
        /// <param name="SerialPortName">The serial port that the machine is on (currently it is in static COM4)</param>
        /// <param name="MachinesAddress">The address of the machine as it is addressable 0 should be the defualt</param>
        public NV200_Com(string SerialPortName, byte MachinesAddress = 0, short PollRateMs = 200, uint TimeOut=1000)
        {
            //if (PollRate < 200 || PollRate > 1000)
            //    throw new Exception($"A poll rate of {PollRate} is too long or short. the Poll rate must be within 200ms to 1000ms as per the documention.");

            ////check if the address is a valid address that the machine can be set for and then set the address with the serial settings
            //    if (MachinesAddress > 15 || MachinesAddress < 0)
            //    throw new Exception(AddressingError);
            //SerialPort = new SerialPort(SerialPortName, BaudRate, Parity.None, DataSize, StopBits.One);
            //this.MachinesAddress = MachinesAddress;

            ComManager = new SSPComms();
            CommandFrame = new SSP_COMMAND();
            keys = new SSP_KEYS();
            sspKey = new SSP_FULL_KEY();
            CInfo = new SSP_COMMAND_INFO();

            //Set static Vars for the Coms info
            CommandFrame.BaudRate = 9600;
            CommandFrame.SSPAddress = MachinesAddress;
            CommandFrame.Timeout = TimeOut;
            CommandFrame.RetryLevel = 1;
            CommandFrame.ComPort = SerialPortName;
            CommandFrame.EncryptionStatus = false;

            //m_NumberOfChannels = 0;
            //m_ValueMultiplier = 1;
            Channels = new List<NV200_ChannelData>();
            //m_HoldCount = 0;
            //m_HoldNumber = 0;
        }

        /// <summary>
        /// Opens the com port for the machine
        /// </summary>
        public void OpenCom()
        {
            SSP_COMMAND CurrentCommand = CommandFrame.CloneBasics();
            if (!ComManager.OpenSSPComPort(CommandFrame))
                throw new Exception("ComManager failed to open its com port. please check that it is not in use.");
            //Sync [11] untill OK [F0] 
        }

        //set vers [06] [07]
        //Get Firmware Version [20] OK, NV02004141498000  [F0] [4E 56 30 32 30 30 34 31 34 31 34 39 38 30 30 30] 
        //Get DatasetVers Get Dataset Version [21] OK, EUR01609  [F0] [45 55 52 30 31 36 30 39]  EUR Euro Country code (ISO 4217) 01 1 Arrangement of dataset, each code has different notes or channel arrangements. See website for details. 6 NV200 Product code for dataset 09 v9 Version number, increased if notes are added, withdrawn or updated. 
        #region SETUP AND ENABLE VALIDATOR
        //Host Protocol Version [06] [06] OK [F0] 
        //Setup Request [05] OK & Setup Data [F0] [00 30 33 33 35 45 55 52 00 00 01 04 05 0A 14 32  02 02 02 02 00 00 64 06 45 55 52 45 55 52  45 55 52 45 55 52  05 00 00 00  0A 00 00 00  14 00 00 00   32 00 00 00]00 = Unit Type (Note Validator) 30 33 33 33 = Firmware (3.33) 45 55 52 = Country Code (EUR) 00 00 01 = Value Multiplier (1) 04 = Number of channels (4) 05 0A 14 32 = Channel Value for  older protocol version – ignore for v6.  02 02 02 02 = Channel Security for  older protocol version – ignore for v6 00 00 64 = Real Value Multiplier(100) 06 = Protocol Version(6) 45 55 52 (repeated x 4) = Currency Code for each channel(EUR) 05 00 00 00 = Value of Ch1(5) 0A 00 00 00 = Value of Ch2(10) 14 00 00 00 = Value of Ch3(20) 32 00 00 00 = Value of Ch4(50)
        //Request Serial Number[0C] OK, & Serial [F0] [00 29 8A A6] 
        //Poll [07] OK, Reset & Disabled [F0] [F1 E8]
        //Set Inhibits [02] [FF FF] OK [F0] 
        //Enable [0A] OK [F0] 
        #endregion
        #region ACCEPT NOTE 
        //Poll[07] OK [F0] 
        //Poll [07] OK & Read Ch 0 [F0] [EF 00] 
        //Poll [07] OK & Note Read Ch 3 [F0] [EF 03] 
        //Hold [18]  OK [F0] //holds for 5 seconds. 0x07 – Poll – Accept Note  0x18 – Hold – Keep the note in escrow position (for 5 seconds longer) 0x08 – Reject – Return the note to the bezel 
        //Poll (Accept) [07]  OK & Stacking [F0] [CC] 
        //Poll[07] OK, Credit Ch. 3 & Stacking  [F0] [EE 03 CC] 
        //Poll [07] OK & Stacking [F0] [CC] 
        //Poll [07] OK & Stacked [F0] [EB]
        //Poll [07] OK [F0]
        #endregion
        //OK, Safe Jam & Disabled [F0] [EA E8] 
        //OK & Cashbox Full [F0] [E7] 

        string SetHostProtocalVers()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_HOST_PROTOCOL_VERSION;
            byte[] RawReturn = SendCommand((byte)Command, new byte[1] { ProtocolVers });
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return ASCIIEncoding.ASCII.GetString(RawReturn, 1, RawReturn.Length - 1);
        }

        string GetFirmWareVers()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_GET_FIRMWARE_VERSION;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return  ASCIIEncoding.ASCII.GetString(RawReturn, 1, RawReturn.Length - 1);
        }

        string GetDataSetVers()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_GET_DATASET_VERSION;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return ASCIIEncoding.ASCII.GetString(RawReturn, 1, RawReturn.Length - 1);
        }

        /// <summary>
        /// Enable disables channels assocated with values of bills
        /// </summary>
        /// <param name="ChannelsToSetEnabled">Flages that you can | to mark channels that you would like to enbled</param>
        /// <returns></returns>
        NV200_Responses SetEnableChannels(NV200_ChannelFlags ChannelsToSetEnabled)
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_SET_CHANNEL_INHIBITS;
            byte[] RawReturn = SendCommand((byte)Command, new byte[2] { (byte)((int)ChannelsToSetEnabled & 0xFF), (byte)(((int)ChannelsToSetEnabled & 0xFF00)>>8) });
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        NV200_Responses Reset()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_RESET;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        NV200_Responses Disable()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_DISABLE;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        NV200_Responses Enable()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_ENABLE;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        NV200_Responses Sync()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_SYNC;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        ///
        /// poll rate should be between 200 - 1000
        /// </summary>
        /// <returns></returns>
        NV200_PollStatusFlags[] PollForStatus()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_POLL;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            NV200_PollStatusFlags[] Return = new NV200_PollStatusFlags[RawReturn.Length - 1];
            for(int i = 1; i < RawReturn.Length; i++)
            {
                Return[i - 1] = (NV200_PollStatusFlags)RawReturn[i];
            }
            return Return;
        }

        NV200_PollStatusFlags[] PollForStatusWithAckRequired()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_POLL_WITH_ACK;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            NV200_PollStatusFlags[] Return = new NV200_PollStatusFlags[RawReturn.Length - 1];
            for (int i = 1; i < RawReturn.Length; i++)
            {
                Return[i - 1] = (NV200_PollStatusFlags)RawReturn[i];
            }
            return Return;
        }

        NV200_Responses AckPoll()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_EVENT_ACK;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        NV200_Responses HoldBankNote()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_HOLD;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        NV200_Responses RejectBankNote()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_REJECT_BANKNOTE;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        NV200_RejectionCodes GetLastRejectCode()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_LAST_REJECT_CODE;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_RejectionCodes)RawReturn[1];
        }

        NV200_Responses SetLightColour(byte RedLevel, byte GreenLevel, byte BlueLevel, bool RememberAfterReset = false)
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_DISPLAY_ON;
            byte[] RawReturn = SendCommand((byte)Command, new byte[4] { RedLevel, GreenLevel, BlueLevel, (byte)(RememberAfterReset ? 0x01 : 0x00) });
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        NV200_Responses TurnLightOn()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_DISPLAY_ON;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        NV200_Responses TurnLightOff()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_DISPLAY_OFF;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        Int32 GetSerialNumber()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_GET_SERIAL_NUMBER;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return BitConverter.ToInt32(RawReturn, 1);
        }

        byte[] GetChannelValueData()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_CHANNEL_VALUE_REQUEST;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            byte[] Return = new byte[RawReturn.Length - 2];
            for (int i = 2; i < RawReturn.Length; i++)
            {
                Return[i - 2] = RawReturn[i];
            }
            return Return;
        }

        NV200_UnitData GetUnitData()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_UNIT_DATA;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            byte ValidatorType = RawReturn[1];
            string VersNumber = ASCIIEncoding.ASCII.GetString(RawReturn, 2, 4);
            string CurrencyCode = ASCIIEncoding.ASCII.GetString(RawReturn, 6, 3);
            byte[] IntConverter = new byte[4];
            Array.Copy(RawReturn, 9, IntConverter, 1, 3);
            int ValueMultiplier = BitConverter.ToInt16(IntConverter, 0);
            byte ProtocolVers = RawReturn[12];
            return new NV200_UnitData(ValidatorType, VersNumber, CurrencyCode, ValueMultiplier, ProtocolVers);
        }

        NV200_SetUpReturn SetUp()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_SETUP_REQUEST;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            byte ValidatorType = RawReturn[1];
            string VersNumber = ASCIIEncoding.ASCII.GetString(RawReturn, 2, 4);
            string CurrencyCode = ASCIIEncoding.ASCII.GetString(RawReturn, 6, 3);
            byte[] IntConverter = new byte[4];
            if (BitConverter.IsLittleEndian)
            {
                Array.Copy(RawReturn, 9, IntConverter, 1, 3);
                IntConverter = IntConverter.Reverse().ToArray();
            }
            else
                Array.Copy(RawReturn, 9, IntConverter, 0, 3);
            int ValueMultiplier = BitConverter.ToInt32(IntConverter, 0);
            byte NumberOfChannels = RawReturn[12];
            IntConverter = new byte[4];
            if (BitConverter.IsLittleEndian)
            {
                Array.Copy(RawReturn, 13 + NumberOfChannels, IntConverter, 1, 3);
                IntConverter = IntConverter.Reverse().ToArray();
            }
            else
                Array.Copy(RawReturn, 13 + NumberOfChannels, IntConverter, 0, 3);
            int RealNumberMultiplier = BitConverter.ToInt32(IntConverter, 0);
            byte ProtocolNumber = RawReturn[16 + NumberOfChannels];
            List<NV200_ChannelData> ChannelData = new List<NV200_ChannelData>(NumberOfChannels);

            for (byte i = 0; i < NumberOfChannels; i++)
            {
                NV200_ChannelData Channel = new NV200_ChannelData();
                Channel.ChannelNumber = (byte)(i + 1);

                //Channel.ChannelValue = RawReturn[13 + i];

                Channel.CurrencyCode = ASCIIEncoding.ASCII.GetString(RawReturn, 17 + NumberOfChannels + i * 3, 3);

                IntConverter = new byte[4];
                Array.Copy(RawReturn, 17 + NumberOfChannels + NumberOfChannels * 3 + i * 4, IntConverter, 0, 4);

                if (!BitConverter.IsLittleEndian)
                    IntConverter = IntConverter.Reverse().ToArray();
                Channel.ChannelValue = BitConverter.ToInt32(IntConverter, 0);

                ChannelData.Add(Channel);
            }

            return new NV200_SetUpReturn(ValidatorType, VersNumber, CurrencyCode, ValueMultiplier, RealNumberMultiplier, ProtocolNumber, ChannelData);
        }

        byte[] SendCommand(byte Command, byte[] Data)
        {
            byte[] Return;
            SSP_COMMAND_INFO CInfo = new SSP_COMMAND_INFO();
            SSP_COMMAND CurrentCommand = CommandFrame.CloneBasics();
            CurrentCommand.CommandDataLength = (byte)(Data.Length + 1);
            CurrentCommand.CommandData[0] = Command;
            for (byte i = 0; i < Data.Length; i++)
            {
                CurrentCommand.CommandData[1 + i] = Data[i];
            }
            if (!ComManager.SSPSendCommand(CurrentCommand, CInfo))
                throw new NV200_CommandException($"The {(NV200_Commands)Command} Command has failed.", CInfo, CurrentCommand);

            Return = new byte[CurrentCommand.ResponseDataLength];
            Array.Copy(CurrentCommand.ResponseData, 0, Return, 0, CurrentCommand.ResponseDataLength);
            this.CInfo = CInfo;
            return Return;
        }
    }
}
