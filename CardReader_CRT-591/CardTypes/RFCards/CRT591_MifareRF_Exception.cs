using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591.RFCards
{
    /// <summary>
    /// An error a RF card can raise
    /// </summary>
    public class CRT591_MifareRF_Exception : Exception
    {
        /// <summary>
        /// The error according to the standards
        /// </summary>
        CRT591_RFCardResponses Error;

        internal CRT591_MifareRF_Exception(CRT591_RFCardResponses Error)
        {
            this.Error = Error;
        }
    }
}
