using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillValidator_NV200
{
    enum NV200_Responses : byte
    {
        SSP_RESPONSE_OK = 0xF0,
        SSP_RESPONSE_COMMAND_NOT_KNOWN = 0xF2,
        SSP_RESPONSE_WRONG_NO_PARAMETERS = 0xF3,
        SSP_RESPONSE_PARAMETER_OUT_OF_RANGE = 0xF4,
        SSP_RESPONSE_COMMAND_CANNOT_BE_PROCESSED = 0xF5,
        SSP_RESPONSE_SOFTWARE_ERROR = 0xF6,
        SSP_RESPONSE_FAIL = 0xF8,
        SSP_RESPONSE_KEY_NOT_SET = 0xFA
    }
}
