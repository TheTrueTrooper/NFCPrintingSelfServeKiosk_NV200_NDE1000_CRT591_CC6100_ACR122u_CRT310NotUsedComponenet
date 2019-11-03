using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    public enum CRT310_CardOperationStatus : byte
    {
        Success = 0x59,
        Failure = 0x4E,
        NoCardToMove = 0x45,
        CardNotARightPosition = 0x57
    }
}
