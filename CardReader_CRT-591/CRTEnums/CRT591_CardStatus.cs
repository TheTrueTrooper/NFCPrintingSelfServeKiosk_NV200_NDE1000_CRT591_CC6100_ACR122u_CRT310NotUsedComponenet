namespace CardReader_CRT_591
{
    /// <summary>
    /// A status Enum returned on reponses for the card track
    /// </summary>
    public enum CRT591_CardStatus : byte
    {
        /// <summary>
        /// We arnt sure what happened but this is an invalid return
        /// </summary>
        CardStatus_Unkown,
        /// <summary>
        /// No Card is in the machine
        /// </summary>
        CardStatus_NoCard = 0x30,
        /// <summary>
        /// Ther is a card in the gate
        /// </summary>
        CardStatus_CardInGate = 0x31,
        /// <summary>
        /// Ther is a card on one of the two write postions
        /// </summary>
        CardStatus_CardInAWritePostion = 0x32
    }
}
