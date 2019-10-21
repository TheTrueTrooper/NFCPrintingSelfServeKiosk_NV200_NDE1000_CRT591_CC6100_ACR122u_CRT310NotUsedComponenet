using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    public class CRT310_ReaderStatus
    {
        public CRT310_CardReaderFrontStatus ReaderFrontStatus { private set; get; }
        public CRT310_CardReaderRearStatus ReaderRearStatus { private set; get; }
        public CRT310_CardStatus CardStatus { private set; get; }

        internal CRT310_ReaderStatus(CRT310_CardStatus CardStatus, CRT310_CardReaderFrontStatus ReaderFrontStatus, CRT310_CardReaderRearStatus ReaderRearStatus)
        {
            this.ReaderFrontStatus = ReaderFrontStatus;
            this.ReaderRearStatus = ReaderRearStatus;
            this.CardStatus = CardStatus;
        }
    }
}
