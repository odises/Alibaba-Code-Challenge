using Shared;

using System;
using System.Threading.Tasks;

namespace Client
{
    public interface IClient : IDisposable
    {
        Task<ResponseMessage> SendAsync(RequestMessage requestMessage);

        //Task<IResponseMessage> SendAsync(IRequestMessage requestMessage);
        //Task<TResponseMessage> SendAsync<TResponseMessage>(IRequestMessage requestMessage) where TResponseMessage : IResponseMessage;
    }
}
