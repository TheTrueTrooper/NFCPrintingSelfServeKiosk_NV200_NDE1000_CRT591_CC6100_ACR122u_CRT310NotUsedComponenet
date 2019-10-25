namespace CardReader_CRT_591.RFCards
{
    /// <summary>
    /// The current air protocol used
    /// </summary>
    public enum CRT591_RFProtocols
    {
        /// <summary>
        /// This card isnt actually in use
        /// </summary>
        Decativated = 0x30,
        /// <summary>
        /// Type A
        /// </summary>
        TypeA = 0x41,
        /// <summary>
        /// Type B
        /// </summary>
        TypeB = 0x42,
        /// <summary>
        /// A protocol that can be used by both A and B due to the drop of the ATS byte
        /// </summary>
        PhilpsMifareOneCardProtocol = 0x4D
    }
}
