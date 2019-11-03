using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    public enum CRT310_ShutterStatus : byte
    {
        ShutterClosed = 0X30,
        ShutterOpen = 0X31
    }
}
