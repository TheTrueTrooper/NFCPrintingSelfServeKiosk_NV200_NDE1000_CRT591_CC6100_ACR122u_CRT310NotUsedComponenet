using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillValidator_NV200
{
    enum NV200_UnitTypes : byte
    {
        Validator = 0x00,
        SMARTHopper = 0x03,
        SMARTPayout = 0x06,
        NV11 = 0x07,
        TEBS = 0x0D
    }
}
