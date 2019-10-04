using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    class CRT591_PositiveResponseMessage : CRT591_BaseResponseMessage
    {
        //ST0 CardStack 
        CRT591_CardStackStatus CardStatus;
        //ST1 CardStack 
        CRT591_CardStackStatus StackStatus;
        //ST2 Error bin status
        CTR591_ErrorCardBinStatus ErrorBinStatus;

        public CRT591_PositiveResponseMessage(byte MachineAddress, byte Command, byte Param, CRT591_CardStackStatus CardStatus, CRT591_CardStackStatus StackStatus, CTR591_ErrorCardBinStatus ErrorBinStatus, byte[] Data = null) : base(CRT591_MessageResponseStatus.Positive, MachineAddress, Command, Param, Data)
        {
            this.CardStatus = CardStatus;
            this.StackStatus = StackStatus;
            this.ErrorBinStatus = ErrorBinStatus;
        }
    }
}
