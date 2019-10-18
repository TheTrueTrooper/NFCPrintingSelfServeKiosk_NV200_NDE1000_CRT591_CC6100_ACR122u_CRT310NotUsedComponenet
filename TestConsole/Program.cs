using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardReader_CRT_591;
using System.IO.Ports;
using System.Threading;
using System.IO;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            #region RealTesting
            //string Com;
            //using (StreamReader SR = new StreamReader($"{Environment.CurrentDirectory}\\Com.txt"))
            //    Com = SR.ReadToEnd();
            //CRT591_Com CardStack = new CRT591_Com(Com);
            //CardStack.OpenCom();

            //string Data;

            //CardStack.SendResetInitCommand(out Data, CRT591_Commands_InitParam.MoveCardToHolding);
            //Console.WriteLine(Data);

            #endregion

            #region SudoTesting
            //byte[] PositiveMessage = new byte[] { 0xF2, 0x00, LENH, LENL, 0x50, 0x30, 0x33, St0, St1, St2, byte[]NData, 0x03, xor };
            //byte[] NegativeResponse = new byte[] { 0xF2, 0x00, LENH, LENL, 0x45, 0x30, 0x33, E1, E0, byte[]NData, 0x03, xor };

            SerialPort Reponse = new SerialPort("Com3", CRT591_Com.BaudRate, Parity.None, CRT591_Com.DataSize, StopBits.One);
            Reponse.Open();

            CRT591_Com CardStacker = new CRT591_Com("Com6");
            CardStacker.OpenCom();
            Task<string> Task = new Task<string>(() => {
                string Out;
                CardStacker.SendResetInitCommand(out Out);
                return Out;
                });
            Task.Start();
            Thread.Sleep(100);
            Reponse.Write(new byte[] { CRT591_Com.ACK }, 0, 1);
            //Negative Rsponse built
            //Reponse.Write(new byte[11] { 0xF2, 0x00, 0x04, 0x08, 0x45, 0x30, 0x33, (byte)'0', (byte)'1', 0x03, 0xAF }, 0, 11);
            //Positive response built with no data
            Reponse.Write(new byte[24] { CRT591_Com.STX, 0x00, 0x00, 0x12, 0x50, 0x30, 0x33, (byte)CRT591_CardStatus.CardStatus_NoCard, (byte)CRT591_CardStackStatus.StackStatus_FewCards, (byte)CTR591_ErrorCardBinStatus.ErrorCardBinStatus_NotFull, 0x43, 0x52, 0x54, 0x2D, 0x35, 0x39, 0x31, 0x2D, 0x4D, 0x52, 0x30, 0x31, CRT591_Com.ETX, 0xE7 }, 0, 24);
            //                                 F2       00     00   12    50    30    33               30                                      31                                                    30                                                43    52    54    2D    35    39    31    2D    4D    52    30    31      03            E7 
            Console.WriteLine(Task.Result);
            #endregion
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
