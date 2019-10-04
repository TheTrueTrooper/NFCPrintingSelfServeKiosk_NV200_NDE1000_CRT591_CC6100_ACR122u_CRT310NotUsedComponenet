using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    class CRT591_BaseResponseMessage
    {

        public CRT591_MessageResponseStatus ResponseStatus { get; private set; }

        public byte MachineAddress { get; private set; }

        public byte Command { get; private set; }

        public byte Param { get; private set; }

        public byte[] Data { get; private set; }

        public CRT591_BaseResponseMessage(CRT591_MessageResponseStatus ResponseStatus = CRT591_MessageResponseStatus.UnkownFormateAssumedNotFor, byte MachineAddress = 0x00, byte Command = 0x00, byte Param = 0x00, byte[] Data = null)
        {
            this.MachineAddress = MachineAddress;
            this.ResponseStatus = ResponseStatus;
            this.Command = Command;
            this.Param = Param;
            this.Data = Data;
        }
    }
}
