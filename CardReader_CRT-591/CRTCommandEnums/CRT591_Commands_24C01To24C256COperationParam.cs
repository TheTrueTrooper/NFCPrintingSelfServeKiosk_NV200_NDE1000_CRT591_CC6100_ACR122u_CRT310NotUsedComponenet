﻿namespace CardReader_CRT_591
{
    internal enum CRT591_Commands_24C01To24C256COperationParam : byte
    {
        ICReset = 0x30,
        ICPowerDown = 0x31,
        ICCheckStatus = 0x32,
        ICRead = 0x33,
        ICWrite = 0x34
    }
}
