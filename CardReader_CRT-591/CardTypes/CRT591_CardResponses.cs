using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    public enum CRT591_CardResponses
    {
        Success = 0x90,
        Fail = 0x6F,
        AddressOverflow = 0x6B,
        OperationLengthOverflow = 0x67
    }
}
