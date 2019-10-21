using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    /// <summary>
    /// A status Enum returned on reponses for the card Error discard bin
    /// </summary>
    public enum CRT310_MessageResponseStatus
    {
        Positive,
        Negative,
        UnkownFormateAssumedNotFor
    }
}
