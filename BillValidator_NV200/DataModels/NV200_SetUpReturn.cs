using System.Collections.Generic;
namespace BillValidator_NV200
{
    public class NV200_SetUpReturn
    {
        public NV200_UnitTypes ValidatorType { get; private set; }
        public string FirmwareVerson { get; private set; }
        public string CurrencyCode { get; private set; }
        public int ValueMultiplier { get; private set; }
        public byte NumberOfChannels { get; private set; }
        public int RealValueMultiplier { get; private set; }
        public byte ProtocolVers { get; private set; }

        public List<NV200_ChannelData> ChannelData { get; private set; }

        internal NV200_SetUpReturn(byte ValidatorType, string FirmwareVerson, string CurrencyCode, int ValueMultiplier, int RealValueMultiplier, byte ProtocolVers, List<NV200_ChannelData> ChannelData)
        {
            this.ValidatorType = (NV200_UnitTypes)ValidatorType;
            this.FirmwareVerson = FirmwareVerson.Insert(2, ".");
            this.CurrencyCode = CurrencyCode;
            this.ValueMultiplier = ValueMultiplier;
            this.RealValueMultiplier = RealValueMultiplier;
            this.ProtocolVers = ProtocolVers;

            if (ChannelData == null)
                ChannelData = new List<NV200_ChannelData>();

            this.ChannelData = ChannelData;

            ChannelData.Sort((x, y) => x.ChannelValue.CompareTo(y));
        }
    }
}
