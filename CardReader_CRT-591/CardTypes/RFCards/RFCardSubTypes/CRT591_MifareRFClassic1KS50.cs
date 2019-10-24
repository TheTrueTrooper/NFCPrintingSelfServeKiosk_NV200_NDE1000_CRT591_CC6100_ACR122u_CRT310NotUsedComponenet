using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591.RFCards
{

    /// <summary>
    /// The class for the S50 chiped mifare classic designed cards with 1K memory
    /// </summary>
    class CRT591_MifareRFClassic1KS50 : CRT591_MifareRF
    {
        const string OutOfRangeExcept = "There are only 16 (0-15) Sectors with 4 (0-3) blocks for a total of 64 (0-63) blocks on a Mifare Classic 1K S50 chip.";
        const string TimeOutError = "Machine has timed out";

        /// <summary>
        /// A int to set your time out periods
        /// </summary>
        public int TimeOutTimeInMS = 10000;

        internal CRT591_MifareRFClassic1KS50(CRT591_Com OwningReader, byte[] UDI, CRT591_RFProtocols Protocol, CRT591_MifareRFTypes CardType, byte ManufacturerSAKValue, byte? ATS = null) : base(OwningReader, UDI, Protocol, CardType, ManufacturerSAKValue, ATS)
        {
        }

        /// <summary>
        /// subracts an int32 from another intager at the address
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Value to Subtract</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
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


        /// <summary>
        /// subracts an int32 from another intager at the address
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Value to Subtract</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
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

        /// <summary>
        /// Add an int32 to another intager at the address
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Value to Add</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
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

        /// <summary>
        /// Add an int32 to another intager at the address
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Value to Add</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
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

        /// <summary>
        /// Reads a sector for it Raw bytes
        /// </summary>
        /// <param name="GlobalBlockAddress"></param>
        /// <returns>The Bytes as an array</returns>
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

        /// <summary>
        /// Reads a sector for it Raw bytes but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <returns>The Bytes as an array</returns>
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

        /// <summary>
        /// Reads an int32 or short from a block
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <returns>The value at the block as an Int32</returns>
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

        /// <summary>
        /// Reads an int32 or short from a block
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <returns>The value at the block as an Int32</returns>
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

        /// <summary>
        /// Writes data to a block
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Data to write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
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

        /// <summary>
        /// Writes data to a block
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Data to write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
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

        /// <summary>
        /// Writes an int32 or short to a block
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Value to write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
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

        /// <summary>
        /// Writes int32 or short to a block
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Value to write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
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
