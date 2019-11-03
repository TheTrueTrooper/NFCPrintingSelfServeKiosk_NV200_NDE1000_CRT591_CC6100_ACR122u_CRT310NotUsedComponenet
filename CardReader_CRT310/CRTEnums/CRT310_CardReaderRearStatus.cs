using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    public enum CRT310_CardReaderRearStatus : byte
    {
        AllowCardRearSide = 0X4A,
        ProhibitCardRearSide = 0x4E
    }
}
