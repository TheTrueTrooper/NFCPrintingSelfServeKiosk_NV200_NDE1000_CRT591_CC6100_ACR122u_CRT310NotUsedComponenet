using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310.Events
{
    /// <summary>
    /// A argument for if a card is placed in the reader
    /// </summary>
    public class CRT310_CardEnteredEventArgs : EventArgs
    {
        /// <summary>
        /// the reader the card was placed in
        /// </summary>
        public CRT310_Com CardReader { private set; get; }

        internal CRT310_CardEnteredEventArgs(CRT310_Com CardReader)
        {
            this.CardReader = CardReader;
        }
    }
}
