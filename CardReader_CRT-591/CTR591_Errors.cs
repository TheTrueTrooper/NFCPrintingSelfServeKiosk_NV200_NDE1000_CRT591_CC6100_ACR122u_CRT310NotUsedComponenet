namespace CardReader_CRT_591
{
    /// <summary>
    /// A set of errors that the system could have
    /// </summary>
    public enum CTR591_Errors
    {
        Error_CommandUndefined = 00,
        Error_CommandParameterError = 01,
        Error_CommandSquenceError = 02,
        Error_CommandNotSupportedByHardware = 03,
        Error_CommandDataError = 04,
        Error_CardContactIssue = 05,
        Error_CardJam = 10,
        Error_SensorError = 12,
        Error_CardTooLong = 13,
        Error_CardTooShort = 14,
        Error_CardRecyclingDisabled = 40,
        Error_CardMagneticRailError = 41,
        Error_CardPostionMoveDisabled = 43,
        Error_CardManuallyMove = 45,
        Error_CardCounterOverflow = 50,
        Error_MotorError = 51,
        Error_CardPowerSupplyShort = 60,
        Error_CardActiviationFailure = 61,
        Error_ICCommandNotSupportedByCard = 62,
        Error_ICCardDisabled = 65,
        Error_ICCommandNotSupportedByCardAtThisTime = 66,
        Error_ICCardTransmittionError = 67,
        Error_ICCardTransmittionOvertime = 68,
        Error_CPUSAMNonEMVStandardCompliance = 69,
        Error_EmptyStacker = 80,     //A0
        Error_ErrorCardBinFull = 81, //A1
        Error_RequireReset = 90,     //B0
        Error_NotMapped = 0xFF
    }
}
