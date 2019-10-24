using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    /// <summary>
    /// A class to encumpase negative messages at their core
    /// </summary>
    public class CRT591_NegativeResponseMessage : CRT591_BaseResponseMessage
    {
        /// <summary>
        /// the error that was described
        /// </summary>
        public CTR591_Errors Error { get; private set; }

        /// <summary>
        /// Create one
        /// </summary>
        /// <param name="MachineAddress">The machine address</param>
        /// <param name="Command">The command that was requested</param>
        /// <param name="Param">the param or sub command that was request</param>
        /// <param name="Error">The error that was generated</param>
        /// <param name="Data">The data on the block</param>
        public CRT591_NegativeResponseMessage(byte MachineAddress, byte Command, byte Param, CTR591_Errors Error, byte[] Data = null) : base(CRT591_MessageResponseStatus.Negative, MachineAddress, Command, Param, Data)
        {
            this.Error = Error;
        }
    }
}
