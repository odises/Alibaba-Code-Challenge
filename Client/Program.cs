using Shared;

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MyTcpClient myTcpClient = new MyTcpClient("127.0.0.1", 10254);

            while (true)
            {
                try
                {
                    var message = Console.ReadLine();
                    var response = await myTcpClient.SendAsync(new RequestMessage { Message = message });
                    Console.WriteLine(response.Message);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
    }
}
