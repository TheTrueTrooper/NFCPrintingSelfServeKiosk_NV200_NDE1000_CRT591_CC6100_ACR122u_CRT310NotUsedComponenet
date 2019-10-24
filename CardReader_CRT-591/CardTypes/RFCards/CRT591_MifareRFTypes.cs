namespace CardReader_CRT_591.RFCards
{
    /// <summary>
    /// All of the types of cards. Used to identify and cast accordingly
    /// </summary>
    public enum CRT591_MifareRFTypes
    {
        /// <summary>
        /// Card is deactivated dont use. Or activate first
        /// </summary>
        DeactivatedRFOrNoCard,
        /// <summary>
        /// Card is a CRT591_MifareRFClassic1KS50
        /// </summary>
        MifareS50 = 0x0004,
        /// <summary>
        /// Card is a CRT591_MifareRFClassic4KS70 (Not currently in support)
        /// </summary>
        MifareS70 = 0x0002,
        /// <summary>
        /// Card is a MifareUL (Not currently in support)
        /// </summary>
        MifareUL = 0x0044,
        /// <summary>
        /// Card is an unknown make Type A (Not currently in support)
        /// </summary>
        TypeA_CPUCard = 0x0101,
        /// <summary>
        /// Card is an unknown make Type B (Not currently in support)
        /// </summary>
        TypeB_CPUCard = 0x0102
    }
}
