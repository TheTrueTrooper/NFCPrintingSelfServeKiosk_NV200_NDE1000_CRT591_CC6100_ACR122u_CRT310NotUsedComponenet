namespace BillValidator_NV200
{
    public class NV200_ChannelData
    {
        public byte ChannelNumber { get; internal set; }
        public int ChannelValue { get; internal set; }
        public NV200_ChannelFlags Channel { get { return (NV200_ChannelFlags)ChannelNumber; } }
        public string CurrencyCode { get; internal set; }
    }
}
