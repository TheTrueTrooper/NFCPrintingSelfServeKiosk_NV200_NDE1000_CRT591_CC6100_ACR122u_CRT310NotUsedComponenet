using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    class CRT591_NegativeResponseMessage : CRT591_BaseResponseMessage
    {
        CTR591_Errors Error;

        public CRT591_NegativeResponseMessage(byte MachineAddress, byte Command, byte Param, CTR591_Errors Error, byte[] Data = null) : base(CRT591_MessageResponseStatus.Negative, MachineAddress, Command, Param, Data)
        {
            this.Error = Error;
        }
    }
}
