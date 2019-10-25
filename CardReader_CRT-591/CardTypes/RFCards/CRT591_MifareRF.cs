using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591.RFCards
{
    /// <summary>
    /// A base class for all RF Cards. It also cab be used to drop card secif checks
    /// </summary>
    public abstract class CRT591_MifareRF : CRT591_IRFCard
    {
        const string DeactivatedError = "Sorry but this card has already been deactivated. Please reactivate at reader before running operations.";

        /// <summary>
        /// This is the owning reader
        /// </summary>
        public CRT591_Com OwningReader { private set; get; }

        /// <summary>
        /// The Unique Device Identification bytes or number 
        /// </summary>
        public byte[] UDI { private set; get; }

        /// <summary>
        /// The protocol that the card is currently using
        /// </summary>
        public CRT591_RFProtocols Protocol { private set; get; }

        /// <summary>
        /// The RF cards type (Mostly from Mifare family)
        /// </summary>
        public CRT591_MifareRFTypes CardType { private set; get; }

        /// <summary>
        /// The manufacturer code
        /// </summary>
        public byte ManufacturerSAKValue { private set; get; }

        /// <summary>
        /// the optional ATS byte that is only included in A protocol
        /// </summary>
        public byte? ATS { private set; get; }

        /// <summary>
        /// The Base cards type in this case a RF Card
        /// </summary>
        public CRT591_CardTypes CardBaseType => CRT591_CardTypes.RFCard;

        /// <summary>
        /// Gets the If the card is still active
        /// </summary>
        public bool Active { private set; get; } = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OwningReader"></param>
        /// <param name="UDI"></param>
        /// <param name="Protocol"></param>
        /// <param name="CardType"></param>
        /// <param name="ManufacturerSAKValue"></param>
        /// <param name="ATS"></param>
        internal CRT591_MifareRF(CRT591_Com OwningReader, byte[] UDI, CRT591_RFProtocols Protocol, CRT591_MifareRFTypes CardType, byte ManufacturerSAKValue, byte? ATS = null)
        {
            this.OwningReader = OwningReader;
            this.UDI = UDI;
            this.Protocol = Protocol;
            this.CardType = CardType;
            this.ATS = ATS;
            this.ManufacturerSAKValue = ManufacturerSAKValue;
        }

        /// <summary>
        /// Deactivates the card
        /// </summary>
        public void DeactivateCard()
        {
            if (!Active)
                throw new Exception(DeactivatedError);
            OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.PowerDown);
            Active = false;
            OwningReader.NotifyOfChildsCardsDestruction();
        }


        /// <summary>
        /// Gets the status of the reader.
        /// </summary>
        /// <returns></returns>
        public CRT591_MifareRFTypes InquireStatusRFID()
        {
            CRT591_PositiveResponseMessage Reponse = OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.OperationStatusCheck);
            string Value = "";
            Value += (char)Reponse.DataRaw[0];
            Value += (char)Reponse.DataRaw[1];

            switch(Value)
            {
                case "00":
                    return CRT591_MifareRFTypes.DeactivatedRFOrNoCard;
                case "10":
                    return CRT591_MifareRFTypes.MifareS50;
                case "11":
                    return CRT591_MifareRFTypes.MifareS70;
                case "12":
                    return CRT591_MifareRFTypes.MifareUL;
                case "20":
                    return CRT591_MifareRFTypes.TypeA_CPUCard;
                case "30":
                    return CRT591_MifareRFTypes.TypeB_CPUCard;
            }
            return CRT591_MifareRFTypes.DeactivatedRFOrNoCard;
        }

        /// <summary>
        /// Validates a sector base on a key given to allow read or write commands
        /// </summary>
        /// <param name="Key">The 6 byte Key to validate against</param>
        /// <param name="Address">The address (if ValidateBySectorVersGobalAddress == true : then it is the Sector address : else it is the block address from the begining (defualt))</param>
        /// <param name="KeySelect">Sets the key to validate as</param>
        /// <param name="ValidateBySectorVersGobalAddress">Sets if the address should be used as a sector address or the block address of the begining</param>
        /// <returns></returns>
        public CRT591_RFCardResponses AthenticateKey(byte[] Key, byte Address, CRT591_RFMifareKeyTypes KeySelect, bool ValidateBySectorVersGobalAddress = false)
        {
            if (!Active)
                throw new Exception(DeactivatedError);
            if (Key == null || Key.Length != 6)
                throw new Exception("Key must be six bytes in length");
            byte[] CardCommandData = new byte[11];
            CardCommandData[0] = 0x00;
            CardCommandData[1] = 0x20;
            CardCommandData[2] = (byte)KeySelect;
            if(ValidateBySectorVersGobalAddress)
                CardCommandData[3] = Address;
            else
                CardCommandData[3] = (byte)(Address / 4);
            CardCommandData[4] = (byte)Key.Length;

            Array.Copy(Key, 0, CardCommandData, 5, Key.Length);

            CRT591_PositiveResponseMessage Reponse = OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.MifareStandardReadWrite, CardCommandData);

            CRT591_RFCardResponses Reuslt = (CRT591_RFCardResponses)Reponse.DataRaw[0];
            if (Reuslt == CRT591_RFCardResponses.Success)
                return Reuslt;

            throw new CRT591_MifareRF_Exception(Reuslt);
        }

        /// <summary>
        /// Reads a Sector for its Raw bytes
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <returns>the Bytes stored there</returns>
        public virtual byte[] Read(byte GlobalBlockAddress)
        {
            return BaseRead(GlobalBlockAddress);
        }

        /// <summary>
        /// Reads a sector for it Raw bytes but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="GlobalBlockAddress"></param>
        /// <returns>the Bytes stored there</returns>
        protected byte[] BaseRead(byte GlobalBlockAddress)
        {
            byte Sector = (byte)(GlobalBlockAddress / 4); 
            byte BlockAddress = (byte)(GlobalBlockAddress % 4);
            return BaseRead(Sector, BlockAddress);
        }

        /// <summary>
        /// Reads a sector for it Raw bytes but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <returns>the Bytes stored there</returns>
        public virtual byte[] Read(byte Sector, byte BlockAddress)
        {
            return BaseRead(Sector, BlockAddress);
        }

        /// <summary>
        /// Reads a sector for it Raw bytes but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="GlobalBlockAddress"></param>
        /// <returns>the Bytes stored there</returns>
        protected byte[] BaseRead(byte Sector, byte BlockAddress)
        {
            if (!Active)
                throw new Exception(DeactivatedError);
            byte[] Return = new byte[16];
            byte[] CardCommandData = new byte[5];
            CardCommandData[0] = 0x00;
            CardCommandData[1] = 0xB0;
            CardCommandData[2] = Sector; // assess reading off of the blocks
            CardCommandData[3] = BlockAddress; 
            CardCommandData[4] = 0x01; //assume one block as this is the way most readers are

            CRT591_PositiveResponseMessage Reponse = OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.MifareStandardReadWrite, CardCommandData);

            CRT591_RFCardResponses Reuslt = (CRT591_RFCardResponses)Reponse.DataRaw[Reponse.DataRaw.Length - 2];

            if (Reuslt == CRT591_RFCardResponses.Success)
            {
                Array.Copy(Reponse.DataRaw, 0, Return, 0, Reponse.DataRaw.Length - 2);
                return Return;
            }

            throw new CRT591_MifareRF_Exception(Reuslt);
        }

        /// <summary>
        /// Writes data to a block
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Data to Write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        public virtual CRT591_RFCardResponses Write(byte GlobalBlockAddress, byte[] Data)
        {
            return BaseWrite(GlobalBlockAddress, Data);
        }

        /// <summary>
        /// Writes data to a block but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Data to Write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        protected CRT591_RFCardResponses BaseWrite(byte GlobalBlockAddress, byte[] Data)
        {
            byte Sector = (byte)(GlobalBlockAddress / 4);
            byte BlockAddress = (byte)(GlobalBlockAddress % 4);
            return BaseWrite(Sector, BlockAddress, Data);
        }

        /// <summary>
        /// Writes data to a block
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Data to Write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        public virtual CRT591_RFCardResponses Write(byte Sector, byte BlockAddress, byte[] Data)
        {
            return BaseWrite(Sector, BlockAddress, Data);
        }

        /// <summary>
        /// Writes data to a block but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Data to Write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        protected CRT591_RFCardResponses BaseWrite(byte Sector, byte BlockAddress, byte[] Data)
        {
            if (!Active)
                throw new Exception(DeactivatedError);
            if (Data == null || Data.Length != 16)
                throw new Exception("Write data must be sixteen bytes in length");
            byte[] CardCommandData = new byte[21];
            CardCommandData[0] = 0x00;
            CardCommandData[1] = 0xD1;
            CardCommandData[2] = Sector; // assess reading off of the blocks
            CardCommandData[3] = BlockAddress;
            CardCommandData[4] = 0x01; //assume one block as this is the way most readers are

            Array.Copy(Data, 0, CardCommandData, 5, Data.Length);

            CRT591_PositiveResponseMessage Reponse = OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.MifareStandardReadWrite, CardCommandData);

            CRT591_RFCardResponses Reuslt = (CRT591_RFCardResponses)Reponse.DataRaw[0];

            if (Reuslt == CRT591_RFCardResponses.Success)
                return Reuslt;

            throw new CRT591_MifareRF_Exception(Reuslt);
        }

        /// <summary>
        /// Writes an int32 or short to a block
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Value to Write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        public virtual CRT591_RFCardResponses WriteValue(byte GlobalBlockAddress, Int32 Data)
        {
            return BaseWriteValue(GlobalBlockAddress, Data);
        }

        /// <summary>
        /// Writes an int32 or short to a block but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Value to Write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        protected CRT591_RFCardResponses BaseWriteValue(byte GlobalBlockAddress, Int32 Data)
        {
            byte Sector = (byte)(GlobalBlockAddress / 4);
            byte BlockAddress = (byte)(GlobalBlockAddress % 4);
            return BaseWriteValue(Sector, BlockAddress, Data);
        }

        /// <summary>
        /// Writes int32 or short to a block
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Value to Write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        public virtual CRT591_RFCardResponses WriteValue(byte Sector, byte BlockAddress, Int32 Data)
        {
            return BaseWriteValue(Sector, BlockAddress, Data);
        }


        /// <summary>
        /// Writes int32 or short to a block but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Value to Write</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        protected CRT591_RFCardResponses BaseWriteValue(byte Sector, byte BlockAddress, Int32 Data)
        {
            if (!Active)
                throw new Exception(DeactivatedError);
            if (Data > Int32.MaxValue || Data < Int32.MinValue)
                throw new Exception("Write data must be within a int sixteen max and min values");
            byte[] CardCommandData = new byte[9];
            CardCommandData[0] = 0x00;
            CardCommandData[1] = 0xD2;
            CardCommandData[2] = Sector; // assess reading off of the blocks
            CardCommandData[3] = BlockAddress;
            CardCommandData[4] = 0x04; //assume one block as this is the way most readers are

            byte[] DataAsBytes = BitConverter.GetBytes(Data);
            if (!BitConverter.IsLittleEndian)
                DataAsBytes = DataAsBytes.Reverse().ToArray();

            Array.Copy(DataAsBytes, 0, CardCommandData, 5, DataAsBytes.Length);

            CRT591_PositiveResponseMessage Reponse = OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.MifareStandardReadWrite, CardCommandData);

            CRT591_RFCardResponses Reuslt = (CRT591_RFCardResponses)Reponse.DataRaw[0];

            if (Reuslt == CRT591_RFCardResponses.Success)
                return Reuslt;

            throw new CRT591_MifareRF_Exception(Reuslt);
        }

        /// <summary>
        /// Reads an int32 or short from a block
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <returns>The value at the block as an Int32</returns>
        public virtual Int32 ReadValue(byte GlobalBlockAddress)
        {
            return BaseReadValue(GlobalBlockAddress);
        }

        /// <summary>
        /// Reads an int32 or short from a block but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        protected Int32 BaseReadValue(byte GlobalBlockAddress)
        {
            byte Sector = (byte)(GlobalBlockAddress / 4);
            byte BlockAddress = (byte)(GlobalBlockAddress % 4);
            return BaseReadValue(Sector, BlockAddress);
        }

        /// <summary>
        /// Reads an int32 or short from a block
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <returns>The value at the block as an Int32(throws on any but true however)</returns>
        public virtual Int32 ReadValue(byte Sector, byte BlockAddress)
        {
            return BaseReadValue(Sector, BlockAddress);
        }

        /// <summary>
        /// Reads an int32 or short from a block but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <returns>The value at the block as an Int32(throws on any but true however)</returns>
        protected Int32 BaseReadValue(byte Sector, byte BlockAddress)
        {
            if (!Active)
                throw new Exception(DeactivatedError);
            byte[] CardCommandData = new byte[4];
            CardCommandData[0] = 0x00;
            CardCommandData[1] = 0xB1;
            CardCommandData[2] = Sector; // assess reading off of the blocks
            CardCommandData[3] = BlockAddress;

            CRT591_PositiveResponseMessage Reponse = OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.MifareStandardReadWrite, CardCommandData);

            CRT591_RFCardResponses Reuslt = (CRT591_RFCardResponses)Reponse.DataRaw[Reponse.DataRaw.Length - 2];


            if (Reuslt == CRT591_RFCardResponses.Success)
            {
                byte[] IntBytes = new byte[4];
                Array.Copy(Reponse.DataRaw, 0, IntBytes, 0, IntBytes.Length);
                if (!BitConverter.IsLittleEndian)
                    IntBytes = IntBytes.Reverse().ToArray();

                return BitConverter.ToInt32(IntBytes, 0);
            }

            throw new CRT591_MifareRF_Exception(Reuslt);
        }

        /// <summary>
        /// Add an int32 to another intager at the address
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Value to Add</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        public virtual CRT591_RFCardResponses IncrementValue(byte GlobalBlockAddress, Int32 Data)
        {
            return BaseIncrementValue(GlobalBlockAddress, Data);
        }

        /// <summary>
        /// Add an int32 to another intager at the address but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Value to Add</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        protected CRT591_RFCardResponses BaseIncrementValue(byte GlobalBlockAddress, Int32 Data)
        {
            byte Sector = (byte)(GlobalBlockAddress / 4);
            byte BlockAddress = (byte)(GlobalBlockAddress % 4);
            return BaseIncrementValue(Sector, BlockAddress, Data);
        }

        /// <summary>
        /// Add an int32 to another intager at the address
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Value to Add</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        public virtual CRT591_RFCardResponses IncrementValue(byte Sector, byte BlockAddress, Int32 Data)
        {
            return BaseIncrementValue(Sector, BlockAddress, Data);
        }

        /// <summary>
        /// Add an int32 to another intager at the address but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Value to Add</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        protected CRT591_RFCardResponses BaseIncrementValue(byte Sector, byte BlockAddress, Int32 Data)
        {
            if (!Active)
                throw new Exception(DeactivatedError);
            if (Data > Int32.MaxValue || Data < Int32.MinValue)
                throw new Exception("Write data must be within a int sixteen max and min values");
            byte[] CardCommandData = new byte[9];
            CardCommandData[0] = 0x00;
            CardCommandData[1] = 0xD3;
            CardCommandData[2] = Sector; // assess reading off of the blocks
            CardCommandData[3] = BlockAddress;
            CardCommandData[4] = 0x04; //assume one block as this is the way most readers are

            byte[] DataAsBytes = BitConverter.GetBytes(Data);
            if (!BitConverter.IsLittleEndian)
                DataAsBytes = DataAsBytes.Reverse().ToArray();

            Array.Copy(DataAsBytes, 0, CardCommandData, 5, DataAsBytes.Length);

            CRT591_PositiveResponseMessage Reponse = OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.MifareStandardReadWrite, CardCommandData);

            CRT591_RFCardResponses Reuslt = (CRT591_RFCardResponses)Reponse.DataRaw[0];

            if (Reuslt == CRT591_RFCardResponses.Success)
                return Reuslt;

            throw new CRT591_MifareRF_Exception(Reuslt);
        }

        /// <summary>
        /// subracts an int32 from another intager at the address
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Value to Subtract</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        public virtual CRT591_RFCardResponses DecrementValue(byte GlobalBlockAddress, Int32 Data)
        {
            return BaseDecrementValue(GlobalBlockAddress, Data);
        }

        /// <summary>
        /// Subtracts an int32 from another int at the address but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="GlobalBlockAddress">The block address relative to the very beginging block</param>
        /// <param name="Data">The Value to Subtract</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        protected CRT591_RFCardResponses BaseDecrementValue(byte GlobalBlockAddress, Int32 Data)
        {
            byte Sector = (byte)(GlobalBlockAddress / 4);
            byte BlockAddress = (byte)(GlobalBlockAddress % 4);
            return BaseDecrementValue(Sector, BlockAddress, Data);
        }

        /// <summary>
        /// Subtracts an int32 from another intager at the address
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Value to Subtract</param>
        /// <returns>the Result of the operation 
        public virtual CRT591_RFCardResponses DecrementValue(byte Sector, byte BlockAddress, Int32 Data)
        {
            return BaseDecrementValue(Sector, BlockAddress, Data);
        }

        /// <summary>
        /// Subtracts an int32 from another intager at the address but for internal use (due to a C# issue where it gets confuesed with base vs this overrides when doing async ops)
        /// </summary>
        /// <param name="Sector">The Sector address</param>
        /// <param name="BlockAddress">The block address relative to the sector</param>
        /// <param name="Data">The Value to Subtract</param>
        /// <returns>the Result of the operation (throws on any but true however)</returns>
        protected CRT591_RFCardResponses BaseDecrementValue(byte Sector, byte BlockAddress, Int32 Data)
        {
            if (!Active)
                throw new Exception(DeactivatedError);
            if (Data > Int32.MaxValue || Data < Int32.MinValue)
                throw new Exception("Write data must be within a int sixteen max and min values");
            byte[] CardCommandData = new byte[9];
            CardCommandData[0] = 0x00;
            CardCommandData[1] = 0xD4;
            CardCommandData[2] = Sector; // assess reading off of the blocks
            CardCommandData[3] = BlockAddress;
            CardCommandData[4] = 0x04; //assume one block as this is the way most readers are

            byte[] DataAsBytes = BitConverter.GetBytes(Data);
            if (!BitConverter.IsLittleEndian)
                DataAsBytes = DataAsBytes.Reverse().ToArray();

            Array.Copy(DataAsBytes, 0, CardCommandData, 5, DataAsBytes.Length);

            CRT591_PositiveResponseMessage Reponse = OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.MifareStandardReadWrite, CardCommandData);

            CRT591_RFCardResponses Reuslt = (CRT591_RFCardResponses)Reponse.DataRaw[0];

            if (Reuslt == CRT591_RFCardResponses.Success)
                return Reuslt;

            throw new CRT591_MifareRF_Exception(Reuslt);
        }

        /// <summary>
        /// Deactivates the card
        /// </summary>
        public void Dispose()
        {
            if (!Active)
            {
                DeactivateCard();
                OwningReader.NotifyOfChildsCardsDestruction();
            }
        }
    }
}
