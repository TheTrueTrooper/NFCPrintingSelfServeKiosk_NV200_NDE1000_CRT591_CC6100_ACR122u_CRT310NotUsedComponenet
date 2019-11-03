using ITLlib;
using System;

namespace BillValidator_NV200
{
    public class NV200_CommandException : Exception
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
