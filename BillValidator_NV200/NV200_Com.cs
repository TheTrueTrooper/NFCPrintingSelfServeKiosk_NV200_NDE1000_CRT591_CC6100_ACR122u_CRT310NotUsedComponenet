﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITLlib;

namespace BillValidator_NV200
{
    public class NV200_Com : IDisposable
    {
        bool Disposed = false;

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

        /// <summary>
        /// Goes through the entire set up proccess
        /// </summary>
        /// <returns></returns>
        public NV200_InitReturn InIt()
        {
            NV200_InitReturn Return = new NV200_InitReturn();
            Sync();
            Return.ProtocolInfoString = SetHostProtocalVers();
            Return.SetUpReturnData = SetUp();
            return Return;
        }

        /// <summary>
        /// Sets the hosts protocol level
        /// </summary>
        /// <returns>info on that</returns>
        string SetHostProtocalVers()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_HOST_PROTOCOL_VERSION;
            byte[] RawReturn = SendCommand((byte)Command, new byte[1] { ProtocolVers });
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return ASCIIEncoding.ASCII.GetString(RawReturn, 1, RawReturn.Length - 1);
        }

        /// <summary>
        /// Simply gets a string containing the firmware info
        /// </summary>
        /// <returns>the firmware info</returns>
        public string GetFirmWareVers()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_GET_FIRMWARE_VERSION;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return  ASCIIEncoding.ASCII.GetString(RawReturn, 1, RawReturn.Length - 1);
        }

        /// <summary>
        /// Simply gets a string containing the datasets info
        /// </summary>
        /// <returns>the datasets info</returns>
        public string GetDataSetVers()
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
        /// <returns>if the call was successfull</returns>
        public NV200_Responses SetEnableChannels(NV200_ChannelFlags ChannelsToSetEnabled)
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_SET_CHANNEL_INHIBITS;
            byte[] RawReturn = SendCommand((byte)Command, new byte[2] { (byte)((int)ChannelsToSetEnabled & 0xFF), (byte)(((int)ChannelsToSetEnabled & 0xFF00)>>8) });
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        /// Resets the machine call after an error
        /// </summary>
        /// <returns>if the call was successfull</returns>
        public NV200_Responses Reset()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_RESET;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        /// Disables the machines operation
        /// </summary>
        /// <returns>if the call was successfull</returns>
        public NV200_Responses Disable()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_DISABLE;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        /// Enables the machine for operation
        /// </summary>
        /// <returns>if the call was successfull</returns>
        public NV200_Responses Enable()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_ENABLE;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        /// syncs the baud rates to eachother call in in it
        /// </summary>
        /// <returns>if the call was successfull</returns>
        public NV200_Responses Sync()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_SYNC;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        /// polls for the EventLogs of the system
        /// poll rate should be between 200 - 1000
        /// </summary>
        /// <returns>A list of events and data that has happened durring the period between polls</returns>
        public List<NV200_PollEvents> PollForStatus()
        {
            List<NV200_PollEvents> Return = new List<NV200_PollEvents>();
            NV200_Commands Command = NV200_Commands.SSP_CMD_POLL;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            for(int i = 1; i < RawReturn.Length; i++)
            {
                NV200_PollEvents Event = new NV200_PollEvents() { EventType = (NV200_PollStatusFlags)RawReturn[i] };

                if (Event.EventType == NV200_PollStatusFlags.Poll_Read_Note || Event.EventType == NV200_PollStatusFlags.Poll_Credit_Note || Event.EventType == NV200_PollStatusFlags.Poll_Fraud_Attempt || Event.EventType == NV200_PollStatusFlags.Poll_Note_Cleared_From_Front || Event.EventType == NV200_PollStatusFlags.Poll_Note_Cleared_To_CashBox)
                {
                    i += 1;
                    Event.Channel = RawReturn[i];
                    break;
                }
                else
                    Event.Channel = null;

                Return.Add(Event);
            }
            return Return;
        }

        /// <summary>
        /// polls for the EventLogs of the system that then requires acknol from the machine
        /// poll rate should be between 200 - 1000
        /// </summary>
        /// <returns>A list of events and data that has happened durring the period between polls</returns>
        public List<NV200_PollEvents> PollForStatusWithAckRequired()
        {
            List<NV200_PollEvents> Return = new List<NV200_PollEvents>();
            NV200_Commands Command = NV200_Commands.SSP_CMD_POLL_WITH_ACK;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            for (int i = 1; i < RawReturn.Length; i++)
            {
                NV200_PollEvents Event = new NV200_PollEvents() { EventType = (NV200_PollStatusFlags)RawReturn[i] };

                if (Event.EventType == NV200_PollStatusFlags.Poll_Read_Note || Event.EventType == NV200_PollStatusFlags.Poll_Credit_Note || Event.EventType == NV200_PollStatusFlags.Poll_Fraud_Attempt || Event.EventType == NV200_PollStatusFlags.Poll_Note_Cleared_From_Front || Event.EventType == NV200_PollStatusFlags.Poll_Note_Cleared_To_CashBox)
                {
                    i += 1;
                    Event.Channel = RawReturn[i];
                    break;
                }
                else
                    Event.Channel = null;

                Return.Add(Event);
            }
            return Return;
        }

        /// <summary>
        /// Simply acknol a poll. Only for use with 'PollForStatusWithAckRequired'
        /// </summary>
        /// <returns>A list of events and data that has happened durring the period between polls</returns>
        public NV200_Responses AckPoll()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_EVENT_ACK;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        /// durring a bill being recived it holds it in the system and prevents it from being taken in
        /// </summary>
        /// <returns>if the call was successfull</returns>
        public NV200_Responses HoldBankNote()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_HOLD;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        /// durring a bill being recived it rejects it automaticly
        /// </summary>
        /// <returns>if the call was successfull</returns>
        public NV200_Responses RejectBankNote()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_REJECT_BANKNOTE;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        /// Gets the last rejection code
        /// </summary>
        /// <returns>The rejections code</returns>
        public NV200_RejectionCodes GetLastRejectCode()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_LAST_REJECT_CODE;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_RejectionCodes)RawReturn[1];
        }

        /// <summary>
        /// Sets the lights intensety and color
        /// </summary>
        /// <param name="RedLevel">The insensity of the red led</param>
        /// <param name="GreenLevel">The insensity of the Green led</param>
        /// <param name="BlueLevel">The insensity of the blue led</param>
        /// <param name="RememberAfterReset"></param>
        /// <returns>if the call was successfull</returns>
        public NV200_Responses SetLightColour(byte RedLevel, byte GreenLevel, byte BlueLevel, bool RememberAfterReset = false)
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_DISPLAY_ON;
            byte[] RawReturn = SendCommand((byte)Command, new byte[4] { RedLevel, GreenLevel, BlueLevel, (byte)(RememberAfterReset ? 0x01 : 0x00) });
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        /// turns the led lights on
        /// </summary>
        /// <returns>if the call was successfull</returns>
        public NV200_Responses TurnLightOn()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_DISPLAY_ON;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        /// turns the led lights off
        /// </summary>
        /// <returns>if the call was successfull</returns>
        public NV200_Responses TurnLightOff()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_DISPLAY_OFF;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            return (NV200_Responses)RawReturn[0];
        }

        /// <summary>
        /// Gets the serial number
        /// </summary>
        /// <returns></returns>
        public Int32 GetSerialNumber()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_GET_SERIAL_NUMBER;
            byte[] RawReturn = SendCommand((byte)Command, new byte[0]);
            if (RawReturn[0] != 0xF0)
                throw new Exception(String.Format(CommandHasFailedError, Command, (NV200_Responses)RawReturn[0]));
            byte[] IntConverter = new byte[4];
            Array.Copy(RawReturn, 1, IntConverter, 0, 4);
            if(BitConverter.IsLittleEndian)
                IntConverter = IntConverter.Reverse().ToArray();
            return BitConverter.ToInt32(IntConverter, 0);
        }

        /// <summary>
        /// Gets the value of each channel as a byte....
        /// </summary>
        /// <returns></returns>
        public byte[] GetChannelValueData()
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

        /// <summary>
        /// gets info to the machines data
        /// </summary>
        /// <returns>Returns info on the machine</returns>
        public NV200_UnitData GetUnitData()
        {
            NV200_Commands Command = NV200_Commands.SSP_CMD_UNIT_DATA;
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
            int ValueMultiplier = BitConverter.ToInt16(IntConverter, 0);
            byte ProtocolVers = RawReturn[12];
            return new NV200_UnitData(ValidatorType, VersNumber, CurrencyCode, ValueMultiplier, ProtocolVers);
        }

        /// <summary>
        /// Goes through the machines init
        /// </summary>
        /// <returns>Returns info on the machine</returns>
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

        /// <summary>
        /// Internal use to make a command
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
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

        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
                this.ComManager.CloseComPort();
            }
        }

        ~NV200_Com()
        {
            Dispose();
        }
    }
}
