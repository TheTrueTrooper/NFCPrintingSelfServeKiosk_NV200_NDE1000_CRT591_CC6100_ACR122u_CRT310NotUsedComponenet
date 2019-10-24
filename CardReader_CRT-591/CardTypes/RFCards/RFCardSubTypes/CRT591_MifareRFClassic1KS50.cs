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
        const string TimeOutError = "Machine has timed out";

        const int TimeOutTimeInMS = 10000;

        internal CRT591_MifareRFClassic1KS50(CRT591_Com OwningReader, byte[] UDI, CRT591_RFProtocols Protocol, CRT591_MifareRFTypes CardType, byte ManufacturerSAKValue, byte? ATS = null) : base(OwningReader, UDI, Protocol, CardType, ManufacturerSAKValue, ATS)
        {
        }

        public override CRT591_RFCardResponses DecrementValue(byte GlobalBlockAddress, int Data)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            Task<CRT591_RFCardResponses> Task = new Task<CRT591_RFCardResponses>(
                () => BaseDecrementValue(GlobalBlockAddress, Data)
                );
            Task.Start();
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result;
        }

        public override CRT591_RFCardResponses DecrementValue(byte Sector, byte BlockAddress, int Data)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);

            Task<CRT591_RFCardResponses> Task = new Task<CRT591_RFCardResponses>(
                ()=> BaseDecrementValue(Sector, BlockAddress, Data)
                );
            Task.Start();
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result; 
        }

        public override CRT591_RFCardResponses IncrementValue(byte GlobalBlockAddress, int Data)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            Task<CRT591_RFCardResponses> Task = new Task<CRT591_RFCardResponses>(
                () => BaseIncrementValue(GlobalBlockAddress, Data)
                );
            Task.Start();
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result;
        }

        public override CRT591_RFCardResponses IncrementValue(byte Sector, byte BlockAddress, int Data)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            Task<CRT591_RFCardResponses> Task = new Task<CRT591_RFCardResponses>(
                () => BaseIncrementValue(Sector, BlockAddress, Data)
                );
            Task.Start();
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result;
        }

        public override byte[] Read(byte GlobalBlockAddress)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            Task<byte[]> Task = new Task<byte[]>(
                () => BaseRead(GlobalBlockAddress)
                );
            Task.Start();
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result;
        }

        public override byte[] Read(byte Sector, byte BlockAddress)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            Task<byte[]> Task = new Task<byte[]>(
                () => BaseRead(Sector, BlockAddress)
                );
            Task.Start();
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result;
        }

        public override int ReadValue(byte GlobalBlockAddress)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            Task<int> Task = new Task<int>(
                () => BaseReadValue(GlobalBlockAddress)
                );
            Task.Start();
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result;
        }

        public override int ReadValue(byte Sector, byte BlockAddress)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            Task<int> Task = new Task<int>(
                () => BaseReadValue(Sector, BlockAddress)
                );
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result;
        }

        public override CRT591_RFCardResponses Write(byte GlobalBlockAddress, byte[] Data)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            Task<CRT591_RFCardResponses> Task = new Task<CRT591_RFCardResponses>(
                () => BaseWrite(GlobalBlockAddress, Data)
                );
            Task.Start();
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result;
        }

        public override CRT591_RFCardResponses Write(byte Sector, byte BlockAddress, byte[] Data)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            Task<CRT591_RFCardResponses> Task = new Task<CRT591_RFCardResponses>(
                () => BaseWrite(Sector, BlockAddress, Data)
                );
            Task.Start();
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result;
        }

        public override CRT591_RFCardResponses WriteValue(byte GlobalBlockAddress, int Data)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (GlobalBlockAddress > 0x3F)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            Task<CRT591_RFCardResponses> Task = new Task<CRT591_RFCardResponses>(
                () => BaseWriteValue(GlobalBlockAddress, Data)
                );
            Task.Start();
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result;
        }

        public override CRT591_RFCardResponses WriteValue(byte Sector, byte BlockAddress, int Data)
        {
            TimeSpan TimeOutTime = TimeSpan.FromMilliseconds(TimeOutTimeInMS);
            if (Sector > 0x0F || BlockAddress > 0x03)
                throw new ArgumentOutOfRangeException(OutOfRangeExcept);
            Task<CRT591_RFCardResponses> Task = new Task<CRT591_RFCardResponses>(
                () => BaseWriteValue(Sector, BlockAddress, Data)
                );
            Task.Start();
            if (!Task.Wait(TimeOutTime))
                throw new TimeoutException(TimeOutError);
            return Task.Result;
        }
    }
}
