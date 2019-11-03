namespace BillDispenser_NDE1000
{
    public enum NDE1000_Status : byte
    {
        Busy = (byte)'w',
        Ready = (byte)'r',
        Error = (byte)'e',
        TestMode = (byte)'t'
    }
}
