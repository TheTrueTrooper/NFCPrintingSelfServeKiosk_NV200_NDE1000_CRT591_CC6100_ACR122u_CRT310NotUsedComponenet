using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    public enum CRT310_Commands_MoveParam
    {
        MoveCardToRFPostion = 0x2E,
        MoveCardToICContactPostion = 0x2F,
        EjectCardFront = 0x30,
        MoveToFront = 0x31,
        MoveToRear = 0x32,
        EjectOutRear = 0x33,
        EjectAbnormal = 0x34
    }
}
