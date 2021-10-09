using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiYi.IoT.SDK.Mqtt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace DiYi.IoT.SDK.Test
{
    public class IotBuilderTest
    {
        [Fact]
        public void CanCreateInstanceAndGetService()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IIoTPublisher, MyProducerService>();
            var builder = new IoTBuilder(services);
            Assert.NotNull(builder);

            var count = builder.Services.Count;
            Assert.Equal(1, count);

            var provider = services.BuildServiceProvider();
            var capPublisher = provider.GetService<IIoTPublisher>();
            capPublisher.PublishAsync("test", "123");
            Assert.NotNull(capPublisher);
        }

        [Fact]
        public void CanAddCapService()
        {
            var services = new ServiceCollection();
            services.AddIoT(x =>
            {
                x.UseMqtt(c =>
                {
                    c.HostName = "broker.emqx.io";
                    c.Port = 1883;
                    c.ClientId = "MQTT:Cloud:Client:IOT:" + Guid.NewGuid().ToString("N");
                    c.RedisConnectionStr = "localhost:6379";
                });
            });

            var builder = services.BuildServiceProvider();

            //var markService = builder.GetService<CapMarkerService>();
            //Assert.NotNull(markService);
        }

        [Fact]
        public void CanOverridePublishService()
        {
            var services = new ServiceCollection();
            services.AddIoT(x =>
            {
                x.UseMqtt(c =>
                {
                    c.HostName = "broker.emqx.io";
                    c.Port = 1883;
                    c.ClientId = "MQTT:Cloud:Client:IOT:" + Guid.NewGuid().ToString("N");
                    c.RedisConnectionStr = "localhost:6379";
                });
            }).AddProducerService<MyProducerService>();

            var thingy = services.BuildServiceProvider()
                .GetRequiredService<IIoTPublisher>() as MyProducerService;

            Assert.NotNull(thingy);
        }


        [Fact]
        public void CanResolveCapOptions()
        {
            var services = new ServiceCollection();
            services.AddIoT(x =>
            {
                x.UseMqtt(c =>
                {
                    c.HostName = "broker.emqx.io";
                    c.Port = 1883;
                    c.ClientId = "MQTT:Cloud:Client:IOT:" + Guid.NewGuid().ToString("N");
                    c.RedisConnectionStr = "localhost:6379";
                });
            });
            var builder = services.BuildServiceProvider();
            var capOptions = builder.GetService<IOptions<IoTOptions>>().Value;
            Assert.NotNull(capOptions);
        }

        private class MyProducerService : IIoTPublisher
        {
            public IServiceProvider ServiceProvider { get; }

            //public AsyncLocal<ICapTransaction> Transaction { get; }

            public Task PublishAsync<T>(string name, T contentObj, string callbackName = null,
                CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task PublishAsync<T>(string name, T contentObj, IDictionary<string, string> optionHeaders = null,
                CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public void Publish<T>(string name, T contentObj, string callbackName = null)
            {
                throw new NotImplementedException();
            }

            public void Publish<T>(string name, T contentObj, IDictionary<string, string> headers)
            {
                throw new NotImplementedException();
            }
        }
    }
}
