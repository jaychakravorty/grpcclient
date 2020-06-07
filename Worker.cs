using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcServer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GrpcClient
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new Greeter.GreeterClient(channel);
                _logger.LogInformation($"Sending ping to {channel.Target}");
                var reply = await client.SayHelloAsync(new HelloRequest { Name = "Ping" });
                _logger.LogInformation($"{reply.Message} recieved at {Environment.MachineName}");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
