namespace CardReader_CRT_591
{
    /// <summary>
    /// What type of card are you checking for
    /// </summary>
    public enum CRT591_Commands_CheckTypeRForICParam
    {
        /// <summary>
        /// Contact pined cards
        /// </summary>
        AutoCheckICType = 0x30,
        /// <summary>
        /// NFC Cards
        /// </summary>
        AutoCheckRFType = 0x31
    }
}
