using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    /// <summary>
    /// A status Enum returned on reponses for the card Error discard bin
    /// </summary>
    public enum CRT591_MessageResponseStatus
    {
        Positive,
        Negative,
        NotForThisInstance,
        UnkownFormateAssumedNotFor
    }
}
