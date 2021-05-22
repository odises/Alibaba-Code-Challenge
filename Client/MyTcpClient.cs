using Shared;

using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client
{
    public class MyTcpClient : IClient
    {
        private readonly NetworkStream _stream;
        private readonly TcpClient _client;

        public MyTcpClient(string ip, int port)
        {
            _client = new(ip, port);

            _stream = _client.GetStream();
        }

        public async Task<ResponseMessage> SendAsync(RequestMessage requestMessage)
        {
            var serializedRequestMessage = JsonSerializer.Serialize(requestMessage);

            byte[] data = Encoding.ASCII.GetBytes(serializedRequestMessage);

            await _stream.WriteAsync(data, 0, data.Length);

            data = new byte[256];

            int bytes = await _stream.ReadAsync(data, 0, data.Length);

            string responseMessage = Encoding.ASCII.GetString(data, 0, bytes);

            if (string.IsNullOrEmpty(responseMessage))
            {
                throw new Exception("Connection closed.");
            }

            var deserializedResponseMessage = JsonSerializer.Deserialize<ResponseMessage>(responseMessage);

            if (deserializedResponseMessage.Message == "-1")
            {
                throw new ArgumentException("Invalid Argument!");
            }

            return deserializedResponseMessage;
        }
        
        public void Dispose()
        {
            _stream.Dispose();
            _client.Dispose();
        }
    }
}