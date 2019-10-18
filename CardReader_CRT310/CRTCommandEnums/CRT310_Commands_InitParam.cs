namespace CardReader_CRT_310
{
    public enum CRT310_Commands_InitParam
    {
        MoveCardToHolding = 0x30,
        CaptureCardToRearSideWithoutCardHolding = 0x31,
        CaptureCardToFrontSideWithoutCardHolding = 0x33,
        DoNotMove = 0x34,
        MoveCardToHoldingWithCounter = 0x35,
        CaptureCardToRearSideWithoutCardHoldingWithCounter = 0x35,
        CaptureCardToFrontSideWithoutCardHoldingWithCounter = 0x37
    }
}
