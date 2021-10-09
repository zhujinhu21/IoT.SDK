using System;
using System.Collections.Generic;
using System.Text;
using MQTTnet.Protocol;

namespace DiYi.IoT.SDK.Mqtt
{
    /// <summary>
    /// 配置
    /// </summary>
    public class MqttConfig
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Server { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; } = 1883;


        public int SslPort { get; set; }

        public int WebSocketPort { get; set; }

        public int SslWebSocketPort { get; set; }
        public string ClientIdPre { get; set; }
        public bool CleanSession { get; set; } = false;

        /// <summary>
        /// 订阅 Topic列表
        /// </summary>
        public Dictionary<string, MqttQualityOfServiceLevel> SubscribeTopicList { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 接收mqtt消息超时时间
        /// </summary>
        public int Timeout { get; set; }
    }
}
