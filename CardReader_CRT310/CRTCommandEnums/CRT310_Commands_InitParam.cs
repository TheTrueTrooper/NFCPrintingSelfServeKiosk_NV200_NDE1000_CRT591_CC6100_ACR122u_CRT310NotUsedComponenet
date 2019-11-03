using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    public enum CRT310_Commands_InitParam : byte
    {
        ResetAndReturnVersion = 0x30,
        ResetWithCardEjectedFront = 0x31,
        ResetWithCardEjectedRear = 0x32 
    }
}
