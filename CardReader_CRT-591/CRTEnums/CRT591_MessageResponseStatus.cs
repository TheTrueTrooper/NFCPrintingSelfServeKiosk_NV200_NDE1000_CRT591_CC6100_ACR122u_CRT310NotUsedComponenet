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
        /// <summary>
        /// this message returned as a successful one
        /// </summary>
        Positive,
        /// <summary>
        /// this message returned because of a failure
        /// </summary>
        Negative,
        /// <summary>
        /// this message returned with a differnt machine addressed.
        /// </summary>
        NotForThisInstance,
        /// <summary>
        /// This seems to have been an error or for a different machine using a different protocol.
        /// </summary>
        UnkownFormateAssumedNotFor
    }
}
