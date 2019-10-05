using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    /// <summary>
    /// A set of errors that the system could have
    /// </summary>
    enum CTR591_Errors
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
        Error_CardNotSupported = 62,
        Error_CardDisabled = 65,
        Error_CommandNotSupportedByCard = 66,
        Error_CardTransmittionError = 67,
        Error_CardTransmittionOvertime = 68,
        Error_NonEMVStandardCompliance = 69,
        Error_EmptyStacker = 80,     //A0
        Error_ErrorCardBinFull = 81, //A1
        Error_RequireReset = 90,     //B0
    }
}
