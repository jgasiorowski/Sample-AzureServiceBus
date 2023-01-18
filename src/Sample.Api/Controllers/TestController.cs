using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.Contracts;
using System.Threading.Tasks;

namespace Sample.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IPublishEndpoint _publish;

        public TestController(IPublishEndpoint publish)
        {
            _publish = publish;
        }

        [HttpPut]
        public async Task PutTestMessage(string value)
        {
            await _publish.Publish(new TestMessage { Value = value });
        }
    }
}
