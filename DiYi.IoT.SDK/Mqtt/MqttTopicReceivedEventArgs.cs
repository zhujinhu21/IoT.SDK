using System;
using System.Collections.Generic;
using System.Text;

namespace DiYi.IoT.SDK.Mqtt
{
    public class MqttTopicReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; }


        public MqttTopicReceivedEventArgs(string topic, string message)
        {
            Topic = topic;
            Message = message;
        }

    }
}
