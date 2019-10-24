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
    public enum CTR591_ErrorCardBinStatus
    {
        /// <summary>
        /// We arnt sure what happened but this is an invalid return
        /// </summary>
        ErrorCardBinStatus_Unkown,
        /// <summary>
        /// The error bin is not full
        /// </summary>
        ErrorCardBinStatus_NotFull = 0x30,
        /// <summary>
        /// The error bin is full
        /// </summary>
        ErrorCardBinStatus_Full = 0x31
    }
}
