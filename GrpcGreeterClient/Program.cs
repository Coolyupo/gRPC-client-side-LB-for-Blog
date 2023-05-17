// just a console sample for grpc client-side loadbalancer
// you may need to change to you dependency injection style for .NET > 5's web/webapi instances. 

using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Core;
using GrpcGreeterClient;
using Grpc.Net.Client.Configuration;
using Grpc.Net.Client.Balancer;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{ 
    private static async Task Main(string[] args)
    {
        string TestDNS = Environment.GetEnvironmentVariable("grpcurl") ?? "http://localhost:5287";
        
        /*
        DnsResolverFactory for dns refresh usage.
         */
        var services = new ServiceCollection();
        services.AddSingleton<ResolverFactory>(
            sp => new DnsResolverFactory(refreshInterval: TimeSpan.FromSeconds(10)));

        /*
        you can also custom a retry policy like 
        https://learn.microsoft.com/en-us/aspnet/core/grpc/retries?view=aspnetcore-6.0#configure-a-grpc-retry-policy
        */
        var defaultMethodConfig = new MethodConfig
        {
            Names = { MethodName.Default },
            RetryPolicy = new RetryPolicy
            {
                MaxAttempts = 5,
                InitialBackoff = TimeSpan.FromSeconds(1),
                MaxBackoff = TimeSpan.FromSeconds(5),
                BackoffMultiplier = 1.5,
                RetryableStatusCodes = { StatusCode.Unavailable }
            }
        };

        /*
        https://learn.microsoft.com/en-us/aspnet/core/grpc/loadbalancing?view=aspnetcore-6.0
        */
        var channel = GrpcChannel.ForAddress(
            TestDNS,
            new GrpcChannelOptions 
            { 
                Credentials = ChannelCredentials.Insecure, 
                ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() },MethodConfigs = {defaultMethodConfig} },
                ServiceProvider = services.BuildServiceProvider()
            });

        var client = new Greeter.GreeterClient(channel);
        while(true)
        {
                var reply = await client.SayHelloAsync(
                                new HelloRequest { Name = "GreeterClient" });
                Console.WriteLine("Greeting: " + reply.Message);
                Task.Delay(1000).Wait();
        }
        
    }
}
