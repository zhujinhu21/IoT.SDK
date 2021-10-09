using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace DiYi.IoT.SDK.Mqtt
{
    public class MqttIoTOptionsExtension : IIoTOptionsExtension
    {
        private readonly Action<MqttMQOptions> _configure;

        public MqttIoTOptionsExtension(Action<MqttMQOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
           
            services.TryAddSingleton<IMqttNetClient, MqttNetClient>();
            services.TryAddSingleton<IMqttHandler, MqttHandler>();

       
            services.Configure(_configure);
            services.AddSingleton<IConfigureOptions<RedisOptions>, ConfigureRedisOptions>();
        }
    }
}
