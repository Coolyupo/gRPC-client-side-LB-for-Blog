using System.Net;
using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly string _hostname;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
        _hostname = Dns.GetHostName();

    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + _hostname
        });
    }
}
