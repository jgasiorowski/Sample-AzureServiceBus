using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Contracts;
using System;
using System.Threading.Tasks;

namespace Sample.Worker.Consumers
{
    internal class TestConsumer : IConsumer<TestMessage>
    {
        private readonly ILogger<TestConsumer> _logger;

        public TestConsumer(ILogger<TestConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TestMessage> context)
        {
            _logger.LogInformation("Test Message: {value}", context.Message.Value);
            await Task.Delay(TimeSpan.FromSeconds(10));
            _logger.LogInformation("Consumed Test Message: {value}", context.Message.Value);
        }
    }
}
