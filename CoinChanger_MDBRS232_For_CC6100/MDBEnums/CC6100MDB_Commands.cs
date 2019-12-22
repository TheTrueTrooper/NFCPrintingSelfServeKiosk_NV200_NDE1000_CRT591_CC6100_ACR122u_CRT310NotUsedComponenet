using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinChanger_MDBRS232_For_CC6100
{
    enum CC6100MDB_Commands : byte
    {
        GetStatusPoll = 0x01,
        ResetBA = 0x02,
        GetBASetupStatus = 0x03,
        ToggleEnableDisableOnBA = 0x04,
        AcceptBill = 0x05,
        RejectBill = 0x06,
        GetStackerInfo = 0x07,
        BASecurityCommand = 0x08,
        BAExpansionCommand = 0x09,
        ResetCC = 0x0A,
        GetCCSetupStatus = 0x0B,
        ToggleEnableDisableOnCC = 0x0C,
        GetCCTubeStatus = 0x0D,
        CCChangeCommand = 0x0E,
        CCExpansionCommand = 0x0F
    }
}
