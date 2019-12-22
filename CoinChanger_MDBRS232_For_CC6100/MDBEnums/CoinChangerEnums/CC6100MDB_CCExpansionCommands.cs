using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinChanger_MDBRS232_For_CC6100.MDBEnums.CoinChangerEnums.CoinChangerEnums
{
    enum CC6100MDB_CCExpansionCommands : byte
    {
        Identification = 0x00,
        FeatureEnable = 0x01,
        Payout = 0x02,
        PayoutStatus = 0x03,
        PayoutValuePoll = 0x04,
        SendDiagnosticStatus = 0x05,
        SendControlledManualFillReport = 0x06,
        SendControlledManualPatoutReport = 0x07,
        FTL_REQToRCV = 0xFA,
        FTL_RetryOrDeny = 0xFB,
        FTL_SendBlock = 0xFC,
        FTL_OkToSend = 0xFD,
        FTL_REQToSend = 0xFE,
        ManufactureDefinedDiagnostics = 0xFF
    }
}
