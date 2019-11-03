namespace CardReader_CRT_591
{
    internal enum CRT591_Commands_CPUOperationParam : byte
    {
        ColdReset = 0x30,
        PowerDown = 0x31,
        StatusCheck = 0x32,
        APDUOnProtocolT0 = 0x33,
        APDUOnProtocolT1 = 0x34,
        HotReset = 0x38,
        AutoAPDU = 0x39
    }
}
