using System;
using System.Collections.Generic;
namespace BillDispenser_NDE1000
{
    /// <summary>
    /// Returns info about what was returned.
    /// </summary>
    public class NDE1000_DispensalCheckReturn
    {
        /// <summary>
        /// the amount despensed 
        /// </summary>
        public int DespensalCount { get; private set; }
        /// <summary>
        /// the error if any that occured.
        /// </summary>
        public NDE1000_Errors Error { get; private set; }
        internal NDE1000_DispensalCheckReturn(int DespensalCount, NDE1000_Errors Error)
        {
            this.DespensalCount = DespensalCount;
            this.Error = Error;
        }
    }
}
