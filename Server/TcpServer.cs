using Shared;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class TcpServer
    {
        TcpListener server = null;

        public TcpServer(string ip, int port)
        {
            var ipAddress = IPAddress.Parse(ip);

            server = new TcpListener(ipAddress, port);

            server.Start();

            StartListener();
        }

        public void StartListener()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    Thread t = new(new ParameterizedThreadStart(HandleClient));

                    t.Start(client);
                }
            }
            catch (SocketException exception)
            {
                Console.WriteLine($"Socket Exception: {exception}");

                server.Stop();
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            var stream = client.GetStream();
            byte[] bytes = new byte[256];
            try
            {
                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    string hex = BitConverter.ToString(bytes);
                    string data = Encoding.ASCII.GetString(bytes, 0, i);

                    var requestMessage = System.Text.Json.JsonSerializer.Deserialize<RequestMessage>(data);

                    Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);

                    switch (requestMessage.Message.Trim())
                    {
                        case "Hello":
                            {
                                Thread.Sleep(1000);
                                SendResponse(stream, "Hi");
                                break;
                            }
                        case "Bye":
                            {
                                stream.Close();
                                client.Close();
                                break;
                            }
                        case "Ping":
                            {
                                SendResponse(stream, "Pong");
                                break;
                            }
                        default:
                            {
                                SendResponse(stream, "-1");
                                break;
                            }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: {0}", exception.ToString());
                client.Close();
            }
        }

        private static void SendResponse(NetworkStream stream, string message)
        {
            var serializedMessage = System.Text.Json.JsonSerializer.Serialize(new ResponseMessage { Message = message });

            byte[] reply = Encoding.ASCII.GetBytes(serializedMessage);

            stream.Write(reply, 0, reply.Length);
        }
    }
}
