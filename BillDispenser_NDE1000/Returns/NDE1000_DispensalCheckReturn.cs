using System;
using System.Collections.Generic;
namespace BillDispenser_NDE1000
{
    public class NDE1000_DispensalCheckReturn
    {
        public int DespensalCount { get; private set; }
        public NDE1000_Errors Error { get; private set; }
        internal NDE1000_DispensalCheckReturn(int DespensalCount, NDE1000_Errors Error)
        {
            this.DespensalCount = DespensalCount;
            this.Error = Error;
        }
    }
}
