using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591.RFCards
{
    /// <summary>
    /// A class interface to encumpase all RF cards (This will identify different cards and their air protocol)
    /// </summary>
    public interface CRT591_IRFCard : CRT591_ICard
    {
        /// <summary>
        /// the air protocol that is currently used by the RF card
        /// </summary>
        CRT591_RFProtocols Protocol { get; }

        /// <summary>
        /// the sub type of RF Card
        /// </summary>
        CRT591_MifareRFTypes CardType { get; }
    }
}
