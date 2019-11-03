using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    /// <summary>
    /// A status Enum returned on reponses for the card track
    /// </summary>
    public enum CRT310_CardStatus : byte
    {
        CardStatus_Unkown,
        WithLongCardInReader = 0X46,
        ShortCardInReader = 0X47,
        CardEjectedFrontSide = 0X48,
        CardFrontSide = 0X49,
        InsideReader = 0X4A,
        InsideContactConnectsWithIC = 0X4B,
        CardRearSidePosition = 0X4C,
        CardEjectedRearSide = 0X4D,
        NoCardInTheReader = 0x4E
    }
}
