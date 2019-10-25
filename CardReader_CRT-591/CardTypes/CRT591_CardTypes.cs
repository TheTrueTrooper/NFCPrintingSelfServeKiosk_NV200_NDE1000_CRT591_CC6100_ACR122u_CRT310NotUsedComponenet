using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    /// <summary>
    /// Gets the card types
    /// </summary>
    public enum CRT591_CardTypes
    {
        /// <summary>
        /// This is an RF or NFC Card (See or cast to the CRT591_MifareRF class) 
        /// </summary>
        RFCard,
        /// <summary>
        /// This is a CPU card with pins
        /// </summary>
        CPUCard,
        /// <summary>
        /// This is a CPU Sam card with pins
        /// </summary>
        SamCard,
        /// <summary>
        /// This is a CPU SLE4442 or SLE4428 card with pins
        /// </summary>
        SLE4442And4428,
        /// <summary>
        /// This is a ICMemory card with pins
        /// </summary>
        ICMemoryCard
    }
}
