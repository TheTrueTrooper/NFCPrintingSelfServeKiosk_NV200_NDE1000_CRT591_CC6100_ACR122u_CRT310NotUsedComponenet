using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    public enum CRT310_CardReaderFrontStatus
    {
        AllowCardByMagneticSignalOpeningShutter = 0X49,
        AllowCardBySwitch = 0X4A,
        AllowCardByMagneticSignal = 0X4B,
        ProhibitcardIn = 0x4E
    }
}
