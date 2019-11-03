using ITLlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillValidator_NV200
{
    class NV200_CommandException : Exception
    {
        public SSP_COMMAND_INFO CInfo { private set; get; }
        public SSP_COMMAND CurrentCommand { private set; get; }

        public NV200_CommandException(string Message, SSP_COMMAND_INFO CInfo, SSP_COMMAND CurrentCommand) : base(Message)
        {
            this.CInfo = CInfo;
            this.CurrentCommand = CurrentCommand;
        }
    }
}
