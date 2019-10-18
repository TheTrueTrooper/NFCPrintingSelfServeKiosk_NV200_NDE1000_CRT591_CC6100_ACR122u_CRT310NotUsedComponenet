using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    /// <summary>
    /// A status Enum returned on reponses for the card track
    /// </summary>
    public enum CRT591_CardStatus
    {
        CardStatus_Unkown,
        CardStatus_NoCard = 0x30,
        CardStatus_CardInGate = 0x31,
        CardStatus_CardInRFPostion = 0x32
    }
}
