using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    enum CRT310_Commands_CardReaderStatusParam : byte
    {
        SensorStatus = 0x2F,
        ReaderStatus = 0x30 
    }
}
