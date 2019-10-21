using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    /// <summary>
    /// A set of errors that the system could have
    /// </summary>
    public enum CRT310_Errors
    {
        Error_CommandUndefined = 0x00,
        Error_CommandParameterError = 0x01,
        Error_CommandCannotEcutedAtThisTime = 0x02,
        Error_CommandDataError = 0x04,
        Error_InputVoltageRangOut = 0x05,
        AbnormalCardDetected = 0x06,
        MainPowerOffOnBackUp = 0x07
    }
}
