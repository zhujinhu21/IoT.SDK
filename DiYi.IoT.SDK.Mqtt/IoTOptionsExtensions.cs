using System;
using System.Collections.Generic;
using System.Text;

namespace DiYi.IoT.SDK.Mqtt
{
    public static class IoTOptionsExtensions
    {
        public static IoTOptions UseMqtt(this IoTOptions options, string hostName)
        {
            return options.UseMqtt(opt => { opt.HostName = hostName; });
        }

        public static IoTOptions UseMqtt(this IoTOptions options, Action<MqttMQOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }
            //configure += x => x.RedisConnectionStr = options.re;
            options.RegisterExtension(new MqttMQIoTOptionsExtension(configure));

            return options;
        }
    }
}
