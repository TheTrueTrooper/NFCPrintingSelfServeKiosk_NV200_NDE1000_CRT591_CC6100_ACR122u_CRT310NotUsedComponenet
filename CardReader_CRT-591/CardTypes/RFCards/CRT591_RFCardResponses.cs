namespace CardReader_CRT_591.RFCards
{
    public enum CRT591_RFCardResponses
    {
        Success = 0x90,
        Fail = 0x6F,
        AddressOverflow = 0x6B,
        OperationLengthOverflow = 0x67
    }
}
