using ITLlib;

namespace BillValidator_NV200
{
    public static class NV200_CommandExtentions
    {
        public static SSP_COMMAND CloneBasics(this SSP_COMMAND This)
        {
            return new SSP_COMMAND() { BaudRate = This.BaudRate, RetryLevel = This.RetryLevel, ComPort = This.ComPort, Timeout = This.Timeout, EncryptionStatus = This.EncryptionStatus, SSPAddress = This.SSPAddress };
        }
    }
}
