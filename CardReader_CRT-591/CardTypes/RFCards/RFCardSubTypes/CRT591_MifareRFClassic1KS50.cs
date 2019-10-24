using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591.RFCards
{
    class CRT591_MifareRFClassic1KS50 : CRT591_MifareRF
    {
        const string OutOfRangeExcept = "There are only 16 (0-15) Sectors with 4 (0-3) blocks for a total of 64 (0-63) blocks on a Mifare Classic 1K S50 chip.";

        internal CRT591_MifareRFClassic1KS50(CRT591_Com OwningReader, byte[] UDI, CRT591_RFProtocols Protocol, CRT591_MifareRFTypes CardType, byte ManufacturerSAKValue, byte? ATS = null) : base(OwningReader, UDI, Protocol, CardType, ManufacturerSAKValue, ATS)
        {
        }

        public override CRT591_RFCardResponses DecrementValue(byte GlobalBlockAddress, int Data)
        {
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.DecrementValue(GlobalBlockAddress, Data);
        }

        public override CRT591_RFCardResponses DecrementValue(byte Sector, byte BlockAddress, int Data)
        {
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.DecrementValue(Sector, BlockAddress, Data);
        }

        public override CRT591_RFCardResponses IncrementValue(byte GlobalBlockAddress, int Data)
        {
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.IncrementValue(GlobalBlockAddress, Data);
        }

        public override CRT591_RFCardResponses IncrementValue(byte Sector, byte BlockAddress, int Data)
        {
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.IncrementValue(Sector, BlockAddress, Data);
        }

        public override byte[] Read(byte GlobalBlockAddress)
        {
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.Read(GlobalBlockAddress);
        }

        public override byte[] Read(byte Sector, byte BlockAddress)
        {
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.Read(Sector, BlockAddress);
        }

        public override int ReadValue(byte GlobalBlockAddress)
        {
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.ReadValue(GlobalBlockAddress);
        }

        public override int ReadValue(byte Sector, byte BlockAddress)
        {
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.ReadValue(Sector, BlockAddress);
        }

        public override CRT591_RFCardResponses Write(byte GlobalBlockAddress, byte[] Data)
        {
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.Write(GlobalBlockAddress, Data);
        }

        public override CRT591_RFCardResponses Write(byte Sector, byte BlockAddress, byte[] Data)
        {
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.Write(Sector, BlockAddress, Data);
        }

        public override CRT591_RFCardResponses WriteValue(byte GlobalBlockAddress, int Data)
        {
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.WriteValue(GlobalBlockAddress, Data);
        }

        public override CRT591_RFCardResponses WriteValue(byte Sector, byte BlockAddress, int Data)
        {
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            return base.WriteValue(Sector, BlockAddress, Data);
        }
    }
}
