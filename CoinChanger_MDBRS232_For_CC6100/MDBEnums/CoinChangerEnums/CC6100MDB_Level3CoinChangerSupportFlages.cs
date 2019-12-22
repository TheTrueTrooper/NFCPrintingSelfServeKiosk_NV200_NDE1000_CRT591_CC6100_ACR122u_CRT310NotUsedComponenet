using System;
namespace CoinChanger_MDBRS232_For_CC6100.MDBEnums.CoinChangerEnums
{
    [Flags]
    enum CC6100MDB_Level3CoinChangerSupportFlages : byte
    {
        AlternativePayoutMethod = 0x01,
        ExtendedDiagnostics = 0x02,
        ControlledManualFillAndPayout = 0x04,
        FileTransportLayer = 0x08,
    }
}
