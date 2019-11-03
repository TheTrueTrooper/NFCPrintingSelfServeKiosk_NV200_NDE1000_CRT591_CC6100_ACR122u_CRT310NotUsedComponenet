using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    public class CRT591_CommandException : Exception
    {
        public CRT591_NegativeResponseMessage ReponseOnException { private set; get; }

        internal CRT591_CommandException(CRT591_NegativeResponseMessage ReponseOnException, string Message) : base(Message)
        {
            this.ReponseOnException = ReponseOnException;
        }

        internal CRT591_CommandException(CRT591_NegativeResponseMessage ReponseOnException) : this(ReponseOnException, $"A negative Response has come back on {ReponseOnException.Command}:{ReponseOnException.Param} for Machine {ReponseOnException.MachineAddress}")
        {
        }
    }
}
