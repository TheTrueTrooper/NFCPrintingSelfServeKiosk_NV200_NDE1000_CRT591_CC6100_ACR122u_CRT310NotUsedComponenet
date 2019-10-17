using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardReader_CRT_591;
using System.IO.Ports;
using System.Threading;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

            SerialPort Reponse = new SerialPort("Com6", CRT591_Com.BaudRate, Parity.None, CRT591_Com.DataSize, StopBits.One);
            Reponse.Open();

            CRT591_Com CardStacker = new CRT591_Com("Com3");
            CardStacker.OpenCom();
            Task Task = new Task(()=>CardStacker.SendResetCommand());
            Task.Start();
            Thread.Sleep(100);
            Reponse.Write(new byte[] { CRT591_Com.ACK }, 0, 1);
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
