namespace CoinChanger_MDBRS232_For_CC6100.MDBEnums
{
    enum CC6100MDB_MDBStatuses : byte
    {
        EscrowRequest = 0x01,
        ChangerPayoutBusy = 0x02,
        NoCredit = 0x03,
        DefectiveTubeSensor = 0x04,
        DoubleArrival = 0x05,
        AcceptorUnplugged = 0x06,
        TubeJam = 0x07,
        ROMChecksumError = 0x08,
        CoinRoutingError = 0x09,
        ChangerBusy = 0x0A,
        ChangerWasReset = 0x0B,
        CoinJam = 0x0C,
        CoinnotRecognizedOrSlug_Returned = 0x21
    }
}
