namespace CardReader_CRT_591
{
    /// <summary>
    /// where should the machine move the card to
    /// </summary>
    public enum CRT591_Commands_MoveCardParam : byte
    {
        /// <summary>
        /// Move the card to the holding bin
        /// </summary>
        MoveCardToHolding = 0x30,
        /// <summary>
        /// Move the card to IC contact reader pins for RF Read/Write operations 
        /// </summary>
        MoveCardToIC = 0x31,
        /// <summary>
        /// Move the card to RF reader for RF Read/Write operations 
        /// </summary>
        MoveCardToRF = 0x32,
        /// <summary>
        /// Eject the card to the error bin
        /// </summary>
        MoveCardToErrorBin = 0x33,
        /// <summary>
        /// Eject the card to out the front
        /// </summary>
        MoveCardToGate = 0x39
    }
}
