using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    public class CRT591_ReaderStatus
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

        internal CRT591_ReaderStatus(CRT591_CardStatus CardStatus, CRT591_CardStackStatus StackStatus, CTR591_ErrorCardBinStatus ErrorBinStatus)
        {
            this.CardStatus = CardStatus;
            this.StackStatus = StackStatus;
            this.ErrorBinStatus = ErrorBinStatus;
        }
    }
}
