namespace CardReader_CRT_591
{
    /// <summary>
    /// At start up what would the Reader do
    /// </summary>
    public enum CRT591_Commands_InitParam : byte
    {
        /// <summary>
        /// Move the card to the holding bin
        /// </summary>
        MoveCardToHolding = 0x30,
        /// <summary>
        /// Eject the card to the error bin
        /// </summary>
        CaptureCardToErrorBin = 0x31,
        /// <summary>
        /// leave the card where it is
        /// </summary>
        DontMoveCard = 0x33,
        /// <summary>
        /// Same irst as the start but reset the counter
        /// </summary>
        MoveCardToHoldingAndRetractCounter = 0x34,
        /// <summary>
        /// Same as second as the start but reset the counter
        /// </summary>
        CaptureCardToErrorBinAndRetractCounter = 0x35,
        /// <summary>
        /// Same as third as the start but reset the counter
        /// </summary>
        DontMoveCardAndRetractCounter = 0x37
    }
}
