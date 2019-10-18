using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_310
{
    /// <summary>
    /// A set of errors that the system could have
    /// </summary>
    public enum CRT310_Errors
    {
        Error_CommandCharacterError = 0x00,
        Error_CommandParameterError = 0x01,
        Error_CommandCanNotBeExecuted = 0x02,
        Error_OutOfHardwareSupportCommand = 0x03,
        Error_CommandDataError = 0x04,
        Error_CardJam = 0x10,
        Error_Shuttererror = 0x11,
        Error_CardTooLong = 0x13,
        Error_CardTooShort = 0x14,
        Error_EEPROMError = 0x15,
        Error_CardPulledOutByForce = 0x16,
        Error_CardJamWhenInsert = 0x17,
        Error_CardNotInsertFromRear = 0x19,
        Error_ReadError_CRCError = 0x20,
        Error_ReadError = 0x21,
        Error_ReadError_OnlySS_ES_LRC = 0x23,
        Error_ReadError_NoDataBlank = 0x24,
        Error_ReadErrorNoSS = 0x26,
        Error_ReadErrorNoES = 0x27,
        Error_ReadErrorLRCError = 0x28,
        Error_PowerDown = 0x30,
        Error_VoltageTooHigh = 0x32,
        Error_VoltageTooLow = 0x33,
        Error_CardPulledWhenRetreating = 0x40,
        Error_ICCardOperationError = 0x41,
        Error_DisableToMoveCardToICCardPosition = 0x43,
        Error_CardWithdrawError = 0x45,
        Error_CardCounterOverflow = 0x50,
        Error_PowerShortOnICCard = 0x60,
        Error_ATRError = 0x61,
        Error_ICCardTypeError = 0x62,
        Error_ICCardDisabled = 0x63,
        Error_Otherthan63 = 0x64,
        Error_SendCPUCommandBeforeATR = 0x65,
        Error_CommandOutOfICCurrentCardSupport = 0x66,
        Error_ICCardNonComplianceToEMVStandard = 0x69,
        Error_UnknownCardType = 0x90,
        Error_NotReceiveResetCommand = 0xA0, //B0
        Error_NotMapped = 0xFF
    }
}
