namespace BillDispenser_NDE1000
{
    /// <summary>
    /// status of the machine
    /// </summary>
    public class NDE1000_StatusReturn
    {
        /// <summary>
        /// the overall status
        /// </summary>
        public NDE1000_Status Status { get; private set; }
        /// <summary>
        /// any error that could have occured
        /// </summary>
        public NDE1000_Errors Error { get; private set; }
        /// <summary>
        /// The state of the start key (as in disable or not)
        /// </summary>
        public NDE1000_KeySettings StarKeyStatus { get; private set; }
        /// <summary>
        /// The state of the clear key (as in disable or not)
        /// </summary>
        public NDE1000_KeySettings ClearKeyStatus { get; private set; }

        internal NDE1000_StatusReturn(NDE1000_Status Status, NDE1000_Errors Error, NDE1000_KeySettings StarKeyStatus, NDE1000_KeySettings ClearKeyStatus)
        {
            this.Status = Status;
            this.Error = Error;
            this.StarKeyStatus = StarKeyStatus;
            this.ClearKeyStatus = ClearKeyStatus;
        }
    }
}
