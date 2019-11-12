namespace BillValidator_NV200
{
    /// <summary>
    /// This is called durring set up
    /// </summary>
    public class NV200_InitReturn
    {
        /// <summary>
        /// Any data required for and inculded in set up
        /// </summary>
        public NV200_SetUpReturn SetUpReturnData { get; internal set; }
        /// <summary>
        /// a string that contains info on the protocol
        /// </summary>
        public string ProtocolInfoString { get; internal set; }
    }
}
