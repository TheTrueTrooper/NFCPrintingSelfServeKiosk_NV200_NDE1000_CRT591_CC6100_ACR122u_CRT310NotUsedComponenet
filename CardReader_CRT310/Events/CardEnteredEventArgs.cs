using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310.Events
{
    public class CardEnteredEventArgs : EventArgs
    {
        public CRT310_Com CardReader { private set; get; }

        internal CardEnteredEventArgs(CRT310_Com CardReader)
        {
            this.CardReader = CardReader;
        }
    }
}
