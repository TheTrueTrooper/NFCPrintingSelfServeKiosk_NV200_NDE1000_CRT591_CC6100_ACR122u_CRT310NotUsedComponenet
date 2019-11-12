/// <summary>
/// Different errors that culd habe happended
/// </summary>
public enum NDE1000_Errors : byte
{
    NoError = 0x30,
    BoBillsDurringDespensalCommand = 0x31,
    Jam = 0x32,
    Chain = 0x33,
    Half = 0x34,
    Short = 0x35,
    BoBillsDurringStartButton = 0x36,
    Double = 0x37,
    OverCount4000pcs = 0x38,
    ReceivingErrorDuringCommnunicationTest = 0x39,
    EncoderError = (byte)'A',
    IRLED_LError = (byte)'B',
    IRLED_RError = (byte)'C',
    IRSensor_LRrror = (byte)'D',
    IRSensor_RRrror = (byte)'F',
    IRSensor_DifferentError = (byte)'G',
    BillLowLevelWarning = (byte)'H',
    LowPowerError = (byte)'I'
}