namespace BillDispenser_NDE1000
{
    /// <summary>
    /// the type of data to clear
    /// </summary>
    public enum NDE1000_ClearData
    {
        AccumulatedDispensedNumber = 0x31,
        ErrorMessage = 0x32,
        ErrorMessageAndCountedNumber = 0x33
    }
}
