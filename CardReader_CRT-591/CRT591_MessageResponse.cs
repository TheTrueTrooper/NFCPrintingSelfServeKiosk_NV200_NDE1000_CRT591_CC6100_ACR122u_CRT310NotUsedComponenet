using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    class CRT591_MessageResponse
    {
        public CRT591_MessageResponseStatus MessageStatus { get; private set; } = CRT591_MessageResponseStatus.UnkownFormateAssumedNotFor;
        public CRT591_MessageResponseCommandHeaderStatus CommandHeaderStatus { get; private set; } = CRT591_MessageResponseCommandHeaderStatus.Unknown;

        CRT591_CardStackStatus CardStatus = CRT591_CardStackStatus.CardStatus_Unkown;
        CRT591_CardStackStatus StackStatus = CRT591_CardStackStatus.StackStatus_Unkown;
        CTR591_ErrorCardBinStatus ErrorBinStatus = CTR591_ErrorCardBinStatus.ErrorCardBinStatus_Unkown;

        byte Command = 0x00;
        byte Param = 0x00;

        public byte ResponsesCommand { get; private set; } = 0x00;

        internal CRT591_MessageResponse(){}

        internal CRT591_MessageResponse(CRT591_MessageResponseStatus MessageStatus)
        {
            this.MessageStatus = MessageStatus;
        }

        internal CRT591_MessageResponse(CRT591_MessageResponseStatus MessageStatus, 
            CRT591_MessageResponseCommandHeaderStatus CommandHeaderStatus, 
            byte Command = 0x00, 
            byte Param = 0x00, 
            CRT591_CardStackStatus CardStatus = CRT591_CardStackStatus.CardStatus_Unkown,
            CRT591_CardStackStatus StackStatus = CRT591_CardStackStatus.StackStatus_Unkown,
            CTR591_ErrorCardBinStatus ErrorBinStatus = CTR591_ErrorCardBinStatus.ErrorCardBinStatus_Unkown) : this(MessageStatus)
        {
            this.CommandHeaderStatus = CommandHeaderStatus;
            this.Command = Command;
            this.Param = Param;
            this.CardStatus = CardStatus;
            this.StackStatus = StackStatus;
            this.ErrorBinStatus = ErrorBinStatus;
        }
    }
}
