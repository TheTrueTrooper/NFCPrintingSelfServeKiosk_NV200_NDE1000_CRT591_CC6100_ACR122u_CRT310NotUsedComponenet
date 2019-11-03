using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillDispenser_NDE1000
{
    public enum NDE1000_ClearData
    {
        AccumulatedDispensedNumber = 0x31,
        ErrorMessage = 0x32,
        ErrorMessageAndCountedNumber = 0x33
    }
}
