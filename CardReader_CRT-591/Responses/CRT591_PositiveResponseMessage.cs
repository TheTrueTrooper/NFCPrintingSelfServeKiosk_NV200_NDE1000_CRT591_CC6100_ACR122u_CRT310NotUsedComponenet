using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    /// <summary>
    /// A class to encumpase positive messages at their core
    /// </summary>
    public class CRT591_PositiveResponseMessage : CRT591_BaseResponseMessage
    {
        //ST0 CardStack 
        /// <summary>
        /// The status of the card track
        /// </summary>
        public CRT591_CardStatus CardStatus { get; private set; }
        //ST1 CardStack 
        /// <summary>
        /// The status of the loading stack
        /// </summary>
        public CRT591_CardStackStatus StackStatus { get; private set; }
        //ST2 Error bin status
        /// <summary>
        /// The status of the errored card bin
        /// </summary>
        public CTR591_ErrorCardBinStatus ErrorBinStatus { get; private set; }

        /// <summary>
        /// Make one
        /// </summary>
        /// <param name="MachineAddress">The machine address</param>
        /// <param name="Command">The command that was requested</param>
        /// <param name="Param">the param or sub command that was request</param>
        /// <param name="CardStatus">The status of the card track</param>
        /// <param name="StackStatus">The status of the loading stack</param>
        /// <param name="ErrorBinStatus">The status of the errored card bin</param>
        /// <param name="Data">The data on the block</param>
        public CRT591_PositiveResponseMessage(byte MachineAddress, byte Command, byte Param, CRT591_CardStatus CardStatus, CRT591_CardStackStatus StackStatus, CTR591_ErrorCardBinStatus ErrorBinStatus, byte[] Data = null) : base(CRT591_MessageResponseStatus.Positive, MachineAddress, Command, Param, Data)
        {
            this.CardStatus = CardStatus;
            this.StackStatus = StackStatus;
            this.ErrorBinStatus = ErrorBinStatus;
        }
    }
}
