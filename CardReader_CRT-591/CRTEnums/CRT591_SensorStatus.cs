using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    public enum CRT591_SensorStatus
    {
        UnknownValueOrReservedS8,
        NoCard = 0x30,
        WithCard = 0x31
    }
}
