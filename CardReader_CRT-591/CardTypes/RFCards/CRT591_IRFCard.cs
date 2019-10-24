using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591.RFCards
{
    public interface CRT591_IRFCard : CRT591_ICard, IDisposable
    {
        CRT591_RFProtocols Protocol { get; }

        CRT591_MifareRFTypes CardType { get; }
    }
}
