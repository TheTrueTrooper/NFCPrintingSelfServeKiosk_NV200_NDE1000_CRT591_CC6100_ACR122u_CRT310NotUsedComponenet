namespace CardReader_CRT_591.RFCards
{
    /// <summary>
    /// Standard A & B protocol responses
    /// </summary>
    public enum CRT591_RFCardResponses
    {
        /// <summary>
        /// operation has succed
        /// </summary>
        Success = 0x90,
        /// <summary>
        /// operation has failed
        /// </summary>
        Fail = 0x6F,
        /// <summary>
        /// operation has due to address overflow
        /// </summary>
        AddressOverflow = 0x6B,
        /// <summary>
        /// operation has timed out?
        /// </summary>
        OperationLengthOverflow = 0x67
    }
}
