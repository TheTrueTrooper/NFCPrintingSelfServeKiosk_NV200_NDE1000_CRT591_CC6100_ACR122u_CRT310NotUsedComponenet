using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_310
{
    public class CRT310_PositiveResponseMessage : CRT310_BaseResponseMessage
    {
        //ST0 CardStack  &ST1 CardStack 
        /// <summary>
        /// The status of the card track
        /// </summary>
        CRT310_CardStatus CardStatus;

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
        public CRT310_PositiveResponseMessage(byte MachineAddress, byte Command, byte Param, CRT310_CardStatus CardStatus, byte[] Data = null) : base(CRT310_MessageResponseStatus.Positive, MachineAddress, Command, Param, Data)
        {
            this.CardStatus = CardStatus;
        }
    }
}
