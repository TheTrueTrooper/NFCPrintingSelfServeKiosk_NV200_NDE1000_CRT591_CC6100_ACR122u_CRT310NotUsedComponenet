namespace CardReader_CRT_310
{
    public enum CRT310_Commands_MifareRFOperationParam
    {
        Startup = 0x30,
        PowerDown = 0x31,
        OperationStatusCheck = 0x32,
        MifareStandardReadWrite = 0x33,
        TypeA_APDUExchange = 0x34,
        TypeB_APDUExchange = 0x35,
        //RFEnableAndDisable = 0x39 only 591
    }
}
