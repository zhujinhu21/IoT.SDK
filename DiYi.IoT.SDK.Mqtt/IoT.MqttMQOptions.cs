using System;
using System.Collections.Generic;
using System.Text;
using MQTTnet.Protocol;

namespace DiYi.IoT.SDK.Mqtt
{
    public class MqttMQOptions
    {
        /// <summary>
        /// The host to connect to.
        /// If you want connect to the cluster, you can assign like “192.168.1.111”
        /// </summary>
        public string HostName { get; set; } = "localhost";

        /// <summary>
        /// Password to use when authenticating to the server.
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// Username to use when authenticating to the server.
        /// </summary>
        public string UserName { get; set; } = "";

        public string ClientId { set; get; }

        public int Timeout { set; get; }
        
        public int Port { get; set; } = 1883;

        public string RedisConnectionStr { set; get; }


        /// <summary>
        /// 订阅 Topic列表
        /// </summary>
        public Dictionary<string, MqttQualityOfServiceLevel> SubscribeTopicList { get; set; }

    }
}
