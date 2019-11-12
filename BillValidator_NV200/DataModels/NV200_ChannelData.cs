namespace BillValidator_NV200
{
    /// <summary>
    /// This holds data for a channel. Bills are mapped to a channel via a value
    /// Ex: 20 bills may be on channel 1 with a channel value of 20
    /// </summary>
    public class NV200_ChannelData
    {
        /// <summary>
        /// The channel number that the bill value is on
        /// </summary>
        public byte ChannelNumber { get; internal set; }
        /// <summary>
        /// The Value of the channels bill
        /// </summary>
        public int ChannelValue { get; internal set; }
        /// <summary>
        /// A map to allow for easy disableing of channels
        /// </summary>
        public NV200_ChannelFlags ChannelEnableFlag { get
            {
                switch (ChannelNumber)
                {
                    default:
                        return NV200_ChannelFlags.UnknowChannelOrStillReading;
                    case 0x01:
                        return NV200_ChannelFlags.Channel1;
                    case 0x02:
                        return NV200_ChannelFlags.Channel2;
                    case 0x03:
                        return NV200_ChannelFlags.Channel3;
                    case 0x04:
                        return NV200_ChannelFlags.Channel4;
                    case 0x05:
                        return NV200_ChannelFlags.Channel5;
                    case 0x06:
                        return NV200_ChannelFlags.Channel6;
                    case 0x07:
                        return NV200_ChannelFlags.Channel7;
                    case 0x08:
                        return NV200_ChannelFlags.Channel8;
                    case 0x09:
                        return NV200_ChannelFlags.Channel9;
                    case 0x0A:
                        return NV200_ChannelFlags.Channel10;
                    case 0x0B:
                        return NV200_ChannelFlags.Channel11;
                    case 0x0C:
                        return NV200_ChannelFlags.Channel12;
                    case 0x0D:
                        return NV200_ChannelFlags.Channel13;
                    case 0x0E:
                        return NV200_ChannelFlags.Channel14;
                    case 0x0F:
                        return NV200_ChannelFlags.Channel15;
                }
            }
        }
        /// <summary>
        /// The currancy of the bill (it may be able to map multiple bill currancy types)
        /// </summary>
        public string CurrencyCode { get; internal set; }
    }
}
