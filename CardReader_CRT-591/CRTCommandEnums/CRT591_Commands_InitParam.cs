namespace CardReader_CRT_591
{
    public enum CRT591_Commands_InitParam
    {
        MoveCardToHolding = 0x30,
        CaptureCardToErrorBin = 0x31,
        DontMoveCard = 0x33,
        MoveCardToHoldingAndRetractCounter = 0x34,
        CaptureCardToErrorBinAndRetractCounter = 0x35,
        DontMoveCardAndRetractCounter = 0x37
    }
}
