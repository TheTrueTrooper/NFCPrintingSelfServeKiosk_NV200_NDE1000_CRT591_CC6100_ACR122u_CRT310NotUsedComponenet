namespace BillValidator_NV200
{
    public class NV200_UnitData
    {
        public NV200_UnitTypes ValidatorType { get; private set; }
        public string VersNumber { get; private set; }
        public string CurrencyCode { get; private set; }
        public int ValueMultiplier { get; private set; }
        public byte ProtocolVers { get; private set; }
        internal NV200_UnitData(byte ValidatorType, string VersNumber, string CurrencyCode, int ValueMultiplier, byte ProtocolVers)
        {
            this.ValidatorType = (NV200_UnitTypes)ValidatorType;
            this.VersNumber = VersNumber.Insert(2, ".");
            this.CurrencyCode = CurrencyCode;
            this.ValueMultiplier = ValueMultiplier;
            this.ProtocolVers = ProtocolVers;
        }
    }
}
