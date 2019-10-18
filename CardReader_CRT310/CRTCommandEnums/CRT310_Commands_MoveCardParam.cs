namespace CardReader_CRT_310
{
    enum CRT310_Commands_MoveCardParam
    {
        MoveCardToFrontSideWithoutCardHolding = 0x30,
        MoveCardToRearSideHoldingCardPosition = 0x31,
        MoveCardToRFIDCardOperationPosition = 0x32,
        MoveCardToICCardOperationPosition = 0x33,
        MoveCardRearSideHoldingCardPosition = 0x34,
        MoveCardToRearSideWithoutHoldingCardPosition = 0x35,
    }
}
