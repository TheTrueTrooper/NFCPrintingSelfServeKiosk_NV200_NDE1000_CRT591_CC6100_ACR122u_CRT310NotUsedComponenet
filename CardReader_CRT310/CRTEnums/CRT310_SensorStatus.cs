using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    public enum CRT310_SensorStatus : byte
    {
        NoCardOnSensor = 0x30,
        CardOnSensor = 0x31
    }
}
