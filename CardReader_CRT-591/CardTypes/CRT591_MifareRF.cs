using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    public class CRT591_MifareRF : CRT591_ICard
    {
        CRT591_Com OwningReader;

        byte[] UDI;

        CRT591_RFProtocols Protocol;

        CRT591_MifareRFTypes CardType;

        byte ManufacturerSAKValue;

        byte? ATS;

        internal CRT591_MifareRF(CRT591_Com OwningReader, byte[] UDI, CRT591_RFProtocols Protocol, CRT591_MifareRFTypes CardType, byte ManufacturerSAKValue, byte? ATS = null)
        {
            this.OwningReader = OwningReader;
            this.UDI = UDI;
            this.Protocol = Protocol;
            this.CardType = CardType;
            this.ATS = ATS;
            this.ManufacturerSAKValue = ManufacturerSAKValue;
        }

        void DeactivateCard()
        {
            OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.PowerDown);
        }

        CRT591_MifareRFTypes InquireStatusRFID()
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

        void VerifyKey(byte[] Key, byte BlockAddress, CRT591_MifareKeyTypes KeySelect)
        {
            if (Key == null || Key.Length != 6)
                throw new Exception("Key must be six bytes in length");
            byte[] CardCommandData = new byte[12];
            CardCommandData[0] = 0x00;
            CardCommandData[1] = 0x20;
            CardCommandData[2] = (byte)KeySelect;
            CardCommandData[3] = (byte)(BlockAddress / 4);
            CardCommandData[4] = (byte)Key.Length;

            Array.Copy(Key, 0, CardCommandData, 5, Key.Length);


            CRT591_PositiveResponseMessage Reponse = OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.MifareStandardReadWrite, CardCommandData);

            CRT591_CardResponses Reuslt = (CRT591_CardResponses)Reponse.DataRaw[0];
            if (Reuslt == CRT591_CardResponses.Success)
                return;

            throw new CRT591_MifareRF_Exception(Reuslt);
        }

        byte[] Read(byte BlockAddress)
        {
            byte[] CardCommandData = new byte[5];
            CardCommandData[0] = 0x00;
            CardCommandData[1] = 0xB0;
            CardCommandData[2] = (byte)(BlockAddress / 4); // assess reading off of the blocks
            CardCommandData[3] = (byte)(BlockAddress % 4); 
            CardCommandData[4] = 0x01; //assume one block as this is the way most readers are


            CRT591_PositiveResponseMessage Reponse = OwningReader.SendRFCardControl(CRT591_Commands_MifareRFOperationParam.MifareStandardReadWrite, CardCommandData);

            CRT591_CardResponses Reuslt = (CRT591_CardResponses)Reponse.DataRaw[0];
            if (Reuslt == CRT591_CardResponses.Success)
            {
                return Reponse.DataRaw;
            }

            throw new CRT591_MifareRF_Exception(Reuslt);
        }
    }
}
