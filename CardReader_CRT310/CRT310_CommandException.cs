using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_310
{
    public class CRT310_CommandException : Exception
    {
        public CRT310_NegativeResponseMessage ReponseOnException { private set; get; }

        public CRT310_CommandException(CRT310_NegativeResponseMessage ReponseOnException, string Message) : base(Message)
        {
            this.ReponseOnException = ReponseOnException;
        }

        public CRT310_CommandException(CRT310_NegativeResponseMessage ReponseOnException) : this(ReponseOnException, $"A negative Response has come back on {ReponseOnException.Command}:{ReponseOnException.Param} for Machine {ReponseOnException.MachineAddress}")
        {
        }
    }
}
