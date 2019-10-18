using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_310
{
    /// <summary>
    /// A base message that contains shard data.
    /// </summary>
    public class CRT310_BaseResponseMessage
    {
        /// <summary>
        /// The response status of a message
        /// </summary>
        public CRT310_MessageResponseStatus ResponseStatus { get; private set; }

        /// <summary>
        /// The machine address
        /// </summary>
        public byte MachineAddress { get; private set; }

        /// <summary>
        /// The command that was requested
        /// </summary>
        public byte Command { get; private set; }

        /// <summary>
        /// the param or sub command that was request
        /// </summary>
        public byte Param { get; private set; }

        /// <summary>
        /// The data on the block
        /// </summary>
        public byte[] DataRaw { get; private set; }

        /// <summary>
        /// build me one
        /// </summary>
        /// <param name="ResponseStatus">The response status of a message</param>
        /// <param name="MachineAddress">The machine address</param>
        /// <param name="Command">The command that was requested</param>
        /// <param name="Param">the param or sub command that was request</param>
        /// <param name="Data">The data on the block</param>
        public CRT310_BaseResponseMessage(CRT310_MessageResponseStatus ResponseStatus = CRT310_MessageResponseStatus.UnkownFormateAssumedNotFor, byte MachineAddress = 0x00, byte Command = 0x00, byte Param = 0x00, byte[] Data = null)
        {
            this.MachineAddress = MachineAddress;
            this.ResponseStatus = ResponseStatus;
            this.Command = Command;
            this.Param = Param;
            DataRaw = Data;
        }
    }
}
