using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardReader_CRT_591;
using System.IO.Ports;
using System.Threading;
using System.IO;
using CardReader_CRT310;
using CardReader_CRT_591.RFCards;
using BillValidator_NV200;
using BillDispenser_NDE1000;
using CoinChanger_MDBRS232_For_CC6100;
using CoinChanger_MDBRS232_For_CC6100.MDBEnums.CAD;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            #region CardStackTesting
            #region RealTesting
            //using (StreamWriter WR = new StreamWriter($"{Environment.CurrentDirectory}\\ErrorOutput.txt"))
            //{
            //    try
            //    {
            //        string Com;
            //        using (StreamReader SR = new StreamReader($"{Environment.CurrentDirectory}\\Com.txt"))
            //            Com = SR.ReadToEnd();
            //        CRT591_Com CardStack = new CRT591_Com(Com);
            //        CardStack.OpenCom();
            //        Thread.Sleep(300);
            //        Console.WriteLine(CardStack.ResetInitCommand(CRT591_Commands_InitParam.MoveCardToHolding));
            //        Thread.Sleep(500);
            //        CardStack.MoveCardCommand(CRT591_Commands_MoveCardParam.MoveCardToRF);
            //        Thread.Sleep(300);
            //        CRT591_MifareRF Card = CardStack.ConnectRFID() as CRT591_MifareRF;
            //        Thread.Sleep(300);
            //        Console.WriteLine($"ConnectResult:\n\tCardType:{Card.CardType}\n\tProtocol:{Card.Protocol}\n\tUDI:{BitConverter.ToString(Card.UDI)}\n\tSAK:{Card.ManufacturerSAKValue}\n\tATS:{(Card.ATS == null ? "Null" : Card.ATS.ToString())}");
            //        WR.WriteLine($"ConnectResult:\n\tCardType:{Card.CardType}\n\tProtocol:{Card.Protocol}\n\tUDI:{BitConverter.ToString(Card.UDI)}\n\tSAK:{Card.ManufacturerSAKValue}\n\tATS:{(Card.ATS == null ? "Null" : Card.ATS.ToString())}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"AthenticateResultOfBlock5:{Card.AthenticateKey(new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 5, CRT591_RFMifareKeyTypes.KeyA)}");
            //        WR.WriteLine($"AthenticateResultOfBlock5:{Card.AthenticateKey(new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 5, CRT591_RFMifareKeyTypes.KeyA)}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"ReadResultResultOfBlock5:{Card.Read(5)}");
            //        WR.WriteLine($"ReadResultResultOfBlock5:{Card.Read(5)}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"WriteResultResultOfBlock5:{Card.Write(5, new byte[16] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })}");
            //        WR.WriteLine($"WriteResultResultOfBlock5:{Card.Write(5, new byte[16] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"\tReadResultResultOfBlock5After:{Card.Read(5)}");
            //        WR.WriteLine($"\tReadResultResultOfBlock5After:{Card.Read(5)}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"WriteResultResultOfBlock5:{Card.Write(5, new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })}");
            //        WR.WriteLine($"WriteResultResultOfBlock5:{Card.Write(5, new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"\tReadResultResultOfBlock5After:{Card.Read(5)}");
            //        WR.WriteLine($"\tReadResultResultOfBlock5After:{Card.Read(5)}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"WriteValueResultResultOfBlock5:{Card.WriteValue(5, 5)}");
            //        WR.WriteLine($"WriteValueResultResultOfBlock5:{Card.WriteValue(5, 5)}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"\tReadValueResultResultOfBlock5After:{Card.ReadValue(5)}");
            //        WR.WriteLine($"\tReadValueResultResultOfBlock5After:{Card.ReadValue(5)}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"IncrementValueResultResultOfBlock5:{Card.IncrementValue(5, 5)}");
            //        WR.WriteLine($"IncrementValueResultResultOfBlock5:{Card.IncrementValue(5, 5)}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"\tReadValueResultResultOfBlock5After:{Card.ReadValue(5)}");
            //        WR.WriteLine($"\tReadValueResultResultOfBlock5After:{Card.ReadValue(5)}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"DecrementValueResultResultOfBlock5:{Card.DecrementValue(5, 5)}");
            //        WR.WriteLine($"DecrementValueResultResultOfBlock5:{Card.DecrementValue(5, 5)}");
            //        Thread.Sleep(300);
            //        Console.WriteLine($"\tReadValueResultResultOfBlock5After:{Card.ReadValue(5)}");
            //        WR.WriteLine($"\tReadValueResultResultOfBlock5After:{Card.ReadValue(5)}");
            //        Thread.Sleep(300);
            //        CardStack.MoveCardCommand(CRT591_Commands_MoveCardParam.MoveCardToGate);
            //    }
            //    catch (Exception e)
            //    {

            //        WR.WriteLine(e);
            //        WR.WriteLine($"\nInner:{e.InnerException}");
            //    }
            //}
            #endregion

            #region SudoTesting
            //SerialPort Reponse = new SerialPort("Com3", CRT591_Com.BaudRate, Parity.None, CRT591_Com.DataSize, StopBits.One);
            //Reponse.Open();
            //CRT591_Com CardStacker = new CRT591_Com("Com6");
            //CardStacker.OpenCom();
            //#endregion
            //#region TestInit
            //Task<string> Task = new Task<string>(() =>
            //{
            //    return CardStacker.ResetInitCommand();
            //});
            //Task.Start();
            //Thread.Sleep(100);
            //Reponse.Write(new byte[] { CRT591_Com.ACK }, 0, 1);
            ////Negative Rsponse built
            ////Reponse.Write(new byte[11] { 0xF2, 0x00, 0x04, 0x08, 0x45, 0x30, 0x33, (byte)'0', (byte)'1', 0x03, 0xAF }, 0, 11);
            ////Positive response built with no data
            //Reponse.Write(new byte[24] { CRT591_Com.STX, 0x00, 0x00, 0x12, 0x50, 0x30, 0x33, (byte)CRT591_CardStatus.CardStatus_NoCard, (byte)CRT591_CardStackStatus.StackStatus_FewCards, (byte)CTR591_ErrorCardBinStatus.ErrorCardBinStatus_NotFull, 0x43, 0x52, 0x54, 0x2D, 0x35, 0x39, 0x31, 0x2D, 0x4D, 0x52, 0x30, 0x31, CRT591_Com.ETX, 0xE7 }, 0, 24);
            ////                                 F2       00     00   12    50    30    33               30                                      31                                                    30                                                43    52    54    2D    35    39    31    2D    4D    52    30    31      03            E7 
            //Console.WriteLine($"initResult:{Task.Result}");
            //#endregion
            //#region TestCardConnect
            //Task<CRT591_ICard> Task2 = new Task<CRT591_ICard>(() =>
            //{
            //    return CardStacker.ConnectRFID();
            //});
            //Task2.Start();
            //Thread.Sleep(100);
            //Reponse.Write(new byte[] { CRT591_Com.ACK }, 0, 1);
            //Reponse.Write(new byte[21] { CRT591_Com.STX, 0x00, 0x00, 0x0F, 0x50, 0x60, 0x30, (byte)CRT591_CardStatus.CardStatus_NoCard, (byte)CRT591_CardStackStatus.StackStatus_FewCards, (byte)CTR591_ErrorCardBinStatus.ErrorCardBinStatus_NotFull, 0x4D, 0x00, 0x04, 0x04, 0xC9, 0x18, 0x83, 0x01, 0x08, CRT591_Com.ETX, 0xD9 }, 0, 21);
            //CRT591_MifareRF Card = Task2.Result as CRT591_MifareRF;
            //Console.WriteLine($"ConnectResult:\n\tCardType:{Card.CardType}\n\tProtocol:{Card.Protocol}\n\tUDI:{BitConverter.ToString(Card.UDI)}\n\tSAK:{Card.ManufacturerSAKValue}\n\tATS:{(Card.ATS == null ? "Null" : Card.ATS.ToString())}");
            //#endregion
            //#region TestAthenticate
            //Task<CRT591_RFCardResponses> Task3 = new Task<CRT591_RFCardResponses>(() =>
            //{
            //    return Card.AthenticateKey(new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 12, CRT591_RFMifareKeyTypes.KeyA);
            //});
            //Task3.Start();
            //Thread.Sleep(100);
            //Reponse.Write(new byte[] { CRT591_Com.ACK }, 0, 1);
            //Reponse.Write(new byte[14] { CRT591_Com.STX, 0x00, 0x00, 0x08, 0x50, 0x60, 0x33, (byte)CRT591_CardStatus.CardStatus_NoCard, (byte)CRT591_CardStackStatus.StackStatus_FewCards, (byte)CTR591_ErrorCardBinStatus.ErrorCardBinStatus_NotFull, 0x90, 0x00, CRT591_Com.ETX, 0x5b }, 0, 14);
            //Console.WriteLine($"AthenticateResult:{Task3.Result}");
            //#endregion
            //#region TestRead
            //Task<byte[]> Task4 = new Task<byte[]>(() =>
            //{
            //    return Card.Read(3);
            //});
            //Task4.Start();
            //Thread.Sleep(100);
            //Reponse.Write(new byte[] { CRT591_Com.ACK }, 0, 1);
            //Reponse.Write(new byte[30] { CRT591_Com.STX, 0x00, 0x00, 0x18, 0x50, 0x60, 0x33, (byte)CRT591_CardStatus.CardStatus_NoCard, (byte)CRT591_CardStackStatus.StackStatus_FewCards, (byte)CTR591_ErrorCardBinStatus.ErrorCardBinStatus_NotFull, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x07, 0x80, 0x69, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x90, 0x00, CRT591_Com.ETX, 0x5a }, 0, 30);
            //Console.WriteLine($"ReadResult:{BitConverter.ToString(Task4.Result)}");
            //#endregion
            //#region TestWrite
            //Task<CRT591_RFCardResponses> Task5 = new Task<CRT591_RFCardResponses>(() =>
            //{
            //    return Card.Write(3, new byte[16] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x07, 0x80, 0x69, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
            //});
            //Task5.Start();
            //Thread.Sleep(100);
            //Reponse.Write(new byte[] { CRT591_Com.ACK }, 0, 1);
            //Reponse.Write(new byte[14] { CRT591_Com.STX, 0x00, 0x00, 0x08, 0x50, 0x60, 0x33, (byte)CRT591_CardStatus.CardStatus_NoCard, (byte)CRT591_CardStackStatus.StackStatus_FewCards, (byte)CTR591_ErrorCardBinStatus.ErrorCardBinStatus_NotFull, 0x90, 0x00, CRT591_Com.ETX, 0x5b }, 0, 14);
            //Console.WriteLine($"WriteResult:{Task5.Result}");
            //#endregion
            //#region TestWrite
            //Task<CRT591_RFCardResponses> Task6 = new Task<CRT591_RFCardResponses>(() =>
            //{
            //    return Card.WriteValue(3, 5);
            //});
            //Task6.Start();
            //Thread.Sleep(100);
            //Reponse.Write(new byte[] { CRT591_Com.ACK }, 0, 1);
            //Reponse.Write(new byte[14] { CRT591_Com.STX, 0x00, 0x00, 0x08, 0x50, 0x60, 0x33, (byte)CRT591_CardStatus.CardStatus_NoCard, (byte)CRT591_CardStackStatus.StackStatus_FewCards, (byte)CTR591_ErrorCardBinStatus.ErrorCardBinStatus_NotFull, 0x90, 0x00, CRT591_Com.ETX, 0x5b }, 0, 14);
            //Console.WriteLine($"WriteValueResult:{Task6.Result}");
            #endregion
            #endregion

            #region CardReaderTesting
            #region SudoTesting
            //SerialPort Reponse = new SerialPort("Com3", CRT310_Com.BaudRate, Parity.None, CRT310_Com.DataSize, StopBits.One);
            //Reponse.Open();

            //CRT310_Com Reader = new CRT310_Com("Com6");
            //Reader.OpenCom();

            //Task<string> Task = new Task<string>(() =>
            //{
            //    return Reader.ResetInitCommand();
            //});
            //Task.Start();
            //Thread.Sleep(100);
            //Reponse.Write(new byte[] { CRT310_Com.ACK }, 0, 1);
            //Reponse.Write(new byte[8] { CRT310_Com.STX, 0x00, 0x03, 0x32, 0x30, 0x59, CRT310_Com.ETX, 0x59 }, 0, 8);
            ////                                  02        00    03    32    30    59          03         59             
            //Console.WriteLine(Task.Result);
            #endregion
            #region RealTesting
            //using (StreamWriter WR = new StreamWriter($"{Environment.CurrentDirectory}\\ErrorOutput.txt"))
            //{
            //    try
            //    {
            //        string Com;
            //        using (StreamReader SR = new StreamReader($"{Environment.CurrentDirectory}\\Com.txt"))
            //            Com = SR.ReadToEnd();
            //        //Create a Reader
            //        CRT310_Com CardReader = new CRT310_Com(Com);
            //        //Open its com port
            //        CardReader.OpenCom();
            //        Thread.Sleep(300);
            //        //Required call to Init
            //        Console.WriteLine(CardReader.ResetInitCommand(CRT310_Commands_InitParam.ResetAndReturnVersion));
            //        Thread.Sleep(300);
            //        CRT310_CardStatus CardStatus = CRT310_CardStatus.NoCardInTheReader;
            //        //Simple poll loop
            //        while (CardStatus == CRT310_CardStatus.NoCardInTheReader)
            //        {
            //            CRT310_ReaderStatus Status = CardReader.ReaderStatus();
            //            CardStatus = Status.CardStatus;
            //            Console.WriteLine(CardStatus.ToString());
            //            Thread.Sleep(300);
            //        }
            //        Thread.Sleep(300);
            //        //Eject card on detect
            //        Console.WriteLine("Ejecting card now");
            //        Console.WriteLine(CardReader.MoveCard(CRT310_Commands_MoveParam.EjectCardFront).ToString());
            //    }
            //    catch (Exception e)
            //    {

            //        WR.WriteLine(e);
            //        Console.WriteLine(e);
            //        WR.WriteLine($"\nInner:{e.InnerException}");
            //        Console.WriteLine($"\nInner:{e.InnerException}");
            //    }
            //}
            #endregion
            #endregion


            #region BillValidator and BillDespens Examples
            ////Open and det up the com
            //NV200_Com BillValidator = new NV200_Com("Com1");
            //BillValidator.OpenCom();
            ////this is important as it sets the machine up and gets all required operation data
            //NV200_InitReturn Settings = BillValidator.InIt();

            ////open channels desired channels and close all others example (open all)
            //NV200_ChannelFlags OpenChannels = 0x00;
            //foreach (NV200_ChannelData Channels in Settings.SetUpReturnData.ChannelData)
            //    OpenChannels |= Channels.ChannelEnableFlag;
            //BillValidator.SetEnableChannels(OpenChannels);

            ////example of seting the lights on the bezal
            //BillValidator.TurnLightOn();
            //BillValidator.SetLightColour(255/*RGB*/, 0, 0, false /*Remember after power down*/);

            ////example of polling recomend a asyn task patter or just opening a thread for it.
            //while(true)
            //{
            //    List<NV200_PollEvents> EventsInBetween = BillValidator.PollForStatus();
            //    foreach(NV200_PollEvents Event in EventsInBetween)
            //    {
            //        switch(Event.EventType)
            //        {
            //            //if rejected do something ~ up to you really but as an example refer to list for full range
            //            case NV200_PollStatusFlags.Poll_Note_Rejected:
            //                Console.WriteLine($"A {Settings.SetUpReturnData.ChannelData[Event.Channel.Value].ChannelValue + Settings.SetUpReturnData.ChannelData[Event.Channel.Value].CurrencyCode} note was rejected! for {BillValidator.GetLastRejectCode()}");
            //                break;
            //            case NV200_PollStatusFlags.Poll_Note_Stacked:
            //                Console.WriteLine($"A {Settings.SetUpReturnData.ChannelData[Event.Channel.Value].ChannelValue + Settings.SetUpReturnData.ChannelData[Event.Channel.Value].CurrencyCode} note was credited and stacked!");
            //                break;
            //        }
            //    }
            //    //minimal sleep time 200~1000 was recomended. (over time could cause the stack to crash on the machine so try to avoid)
            //    Thread.Sleep(200);
            //}

            ////make and open com
            //NDE1000_Com Despenser = new NDE1000_Com("ComX");
            //Despenser.OpenCom();
            ////lock all buttons (default is all) or know unlock whatever
            //Despenser.LockButtons();
            //const int ExpectedBillValue = 5;
            //int ValueToDespesne = 10;
            //int NumberToDespense = ValueToDespesne / ExpectedBillValue;

            ////despese notes
            //if(NumberToDespense != Despenser.DispenseNotes(NumberToDespense))
            //{
            //    // if it doesnt mach then we have and error
            //    NDE1000_DispensalCheckReturn StatusOfOrder = Despenser.CheckDispensalStatus();
            //    Console.WriteLine($"only {StatusOfOrder.DespensalCount} bills were dispensed due to {StatusOfOrder.Error}");
            //    //You can also use "Despenser.GetStatus()" for MoreInfo
            //}

            //ther are more fuctions like write a time to rtc clock

            #endregion

            #region CoinTest
            CC6100MDB_Com CoinChanger = new CC6100MDB_Com("Com1");

            CoinChanger.OpenCom();

            CoinChanger.PayoutCoin(CC6100MDB_CADScalingFactors.Nickels);
            CoinChanger.PayoutCoin(CC6100MDB_CADScalingFactors.Dimes);
            CoinChanger.PayoutCoin(CC6100MDB_CADScalingFactors.Quarters);
            CoinChanger.PayoutCoin(CC6100MDB_CADScalingFactors.Loonies);
            CoinChanger.PayoutCoin(CC6100MDB_CADScalingFactors.Toonies);
            #endregion

            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
