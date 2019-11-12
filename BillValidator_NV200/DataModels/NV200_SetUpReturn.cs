using System.Collections.Generic;
namespace BillValidator_NV200
{
    /// <summary>
    /// The Setups return and required operation data
    /// </summary>
    public class NV200_SetUpReturn
    {
        /// <summary>
        /// the type of device (N200 is just a validator)
        /// </summary>
        public NV200_UnitTypes ValidatorType { get; private set; }
        /// <summary>
        /// The firmware vers of the device
        /// </summary>
        public string FirmwareVerson { get; private set; }
        /// <summary>
        /// the evens currancy code
        /// </summary>
        public string CurrencyCode { get; private set; }
        /// <summary>
        /// the Value multiplier
        /// </summary>
        public int ValueMultiplier { get; private set; }
        /// <summary>
        /// The overall number of channels
        /// </summary>
        public byte NumberOfChannels { get; private set; }
        /// <summary>
        /// the Real value multi (Ex * 100 for cents)
        /// </summary>
        public int RealValueMultiplier { get; private set; }
        /// <summary>
        /// The protocol vers
        /// </summary>
        public byte ProtocolVers { get; private set; }
        /// <summary>
        /// all the data on the channels
        /// </summary>

        public NV200_ChannelSelector ChannelData { get; private set; }

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

            this.ChannelData = new NV200_ChannelSelector(ChannelData);

            ChannelData.Sort((x, y) => x.ChannelValue.CompareTo(y));
        }
    }
}
