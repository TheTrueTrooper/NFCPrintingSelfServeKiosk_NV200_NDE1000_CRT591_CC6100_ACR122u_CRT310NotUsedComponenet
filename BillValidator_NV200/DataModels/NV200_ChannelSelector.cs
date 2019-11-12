using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillValidator_NV200
{
    /// <summary>
    /// A optional storage container for all of the channels that we can use
    /// </summary>
    public class NV200_ChannelSelector : IEnumerable<NV200_ChannelData>
    {
        /// <summary>
        /// List of all the channels 
        /// </summary>
        public List<NV200_ChannelData> Channels { private set; get; }

        /// <summary>
        /// Gets the bill channel based on machines maped channel number
        /// </summary>
        /// <param name="index">the number that the bill is on base on the index</param>
        /// <returns>the channel by its index from the machine</returns>
        public NV200_ChannelData this[byte index]
        {
            get => Channels.FirstOrDefault(x=> x.ChannelNumber == index);
        }

        /// <summary>
        /// Gets the channel based on its value and currancy code
        /// Format is value than currancy and is not case or space sensitive and > 20USD or 20 usd
        /// </summary>
        /// <param name="Value">the value and currancy as string > Format: 20USD</param>
        /// <returns></returns>
        public NV200_ChannelData this[string Value]
        {
            get => Channels.FirstOrDefault(x => x.ChannelValue + x.CurrencyCode.ToUpper() == Value.ToUpper().Replace(" ", ""));
        }

        /// <summary>
        /// Creates a container to use with your channels
        /// </summary>
        /// <param name="Channels"></param>
        public NV200_ChannelSelector(List<NV200_ChannelData> Channels)
        {
            this.Channels = Channels;
        }

        public IEnumerator<NV200_ChannelData> GetEnumerator()
        {
            return Channels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Channels.GetEnumerator();
        }
    }
}
