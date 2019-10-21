using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    public class CRT310_NegativeResponseMessage : CRT310_BaseResponseMessage
    {
        /// <summary>
        /// the error that was described
        /// </summary>
        CRT310_Errors Error;

        /// <summary>
        /// Create one
        /// </summary>
        /// <param name="MachineAddress">The machine address</param>
        /// <param name="Command">The command that was requested</param>
        /// <param name="Param">the param or sub command that was request</param>
        /// <param name="Error">The error that was generated</param>
        /// <param name="Data">The data on the block</param>
        public CRT310_NegativeResponseMessage(byte Command, CRT310_Errors Error, byte[] Data = null) : base(CRT310_MessageResponseStatus.Negative, Command, Data)
        {
            this.Error = Error;
        }
    }
}
