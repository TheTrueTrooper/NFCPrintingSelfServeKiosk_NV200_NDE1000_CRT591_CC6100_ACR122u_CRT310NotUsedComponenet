namespace CardReader_CRT_591
{
    /// <summary>
    /// Set the card readers Gate to Allow/Disable Entry
    /// </summary>
    public enum CRT591_Commands_SetCardEntryParam
    {
        /// <summary>
        /// Enable card entrys from the outside
        /// </summary>
        EnableCardEntryFromOutput = 0x30,
        /// <summary>
        /// Disable card entrys from the outside
        /// </summary>
        DisableCardEntryFromOutput = 0x31
    }
}
