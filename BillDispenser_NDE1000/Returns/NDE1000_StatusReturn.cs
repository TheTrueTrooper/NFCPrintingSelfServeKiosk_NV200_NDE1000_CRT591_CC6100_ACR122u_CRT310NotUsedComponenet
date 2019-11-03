namespace BillDispenser_NDE1000
{
    public class NDE1000_StatusReturn
    {
        public NDE1000_Status Status { get; private set; }
        public NDE1000_Errors Error { get; private set; }
        public NDE1000_KeySettings StarKeyStatus { get; private set; }
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
