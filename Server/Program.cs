using System;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(delegate ()
            {
                TcpServer myserver = new TcpServer("127.0.0.1", 10254);
            });
            t.Start();

            Console.WriteLine("Server Started...!");
            Console.ReadKey();
        }
    }
}
