# Alibaba code challenge
Write a component that can send a string message asynchronously and receive a response to and from a server.
## What should be implemented
### The server should be able to respond to the following messages:
- Request: "Hello" Response: "Hi" after a one second delay.
- Request: "Bye" Response: "Bye" The server closes the client connection.
- Request: "Ping" Response "Pong"
- Any other message should be considered invalid and an exception should be raised on the client.

### The client and server must maintain an active connection
The underlying protocol is of your choice as long as a single stable network connection is used.
### The implementation should be thread safe
Simultaneous send operations should be completed successfully.
### Interface
The client should implement the following interface:
```csharp
interface IClient
{
    Task<string> SendAsync(string message);
}
```
## Bonus implementation 1
Client requests should be sent down the wire as soon as possible, this means a previous request shouldn't block the client from sending new ones to the server. The server then processes the requests and sends back the response.
## Bonus implementation 2
Implement the following interface instead of the basic one:
```csharp
interface IClient
{
    Task<IResponseMessage> SendAsync(IRequestMessage requestMessage);
    Task<TResponseMessage> SendAsync<TResponseMessage>(IRequestMessage requestMessage) where TResponseMessage: IResponseMessage;
}
```
