using System;
using System.Collections.Generic;
using System.Text;
using DiYi.IoT.SDK.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NewLife.Caching;

namespace DiYi.IoT.SDK.Mqtt
{
    internal sealed class MqttMQIoTOptionsExtension : IIoTOptionsExtension
    {
        private readonly Action<MqttMQOptions> _configure;

        public MqttMQIoTOptionsExtension(Action<MqttMQOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {

            services.Configure(_configure);

            services.TryAddSingleton(x =>
            {
                var options = x.GetService<IOptions<MqttMQOptions>>().Value;
                return options;
            });

            services.TryAddSingleton(x =>
            {
                var options = x.GetService<IOptions<MqttMQOptions>>().Value;
                var rds = new FullRedis { Timeout = 1000 * 20 };
                rds.Init(options.RedisConnectionStr);
                return rds;
            });
            services.AddSingleton<ITransport, MqttMQTransport>();

            services.TryAddSingleton<IMqttNetClient, MqttNetClient>();
            services.TryAddSingleton<IMqttHandler, MqttHandler>();

        }
    }
}
