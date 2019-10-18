namespace CardReader_CRT_310
{
    enum CRT310_Commands_SetCardEntryParam
    {
        ProhibitEntryFromFront = 0x30,
        EnableEntryBySwitch = 0x31,
        EnableEntryNyMagneticSignal = 0x32,
        ProhibitEntryFromRear = 0x40,
        EnableEntryFromRear = 0x41,
    }
}
