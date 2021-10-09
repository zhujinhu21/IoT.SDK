using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Protocol;

namespace DiYi.IoT.SDK.Mqtt
{
    public interface IMqttNetClient
    {
        /// <summary>
        /// 获取连接Id
        /// </summary>
        /// <returns></returns>
        string GetClintId();

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        MqttConfig GetMqttConfig();

        /// <summary>
        /// 连接成功 
        /// </summary>
        event EventHandler<MqttClientConnectedEventArgs> OnConnected;

        /// <summary>
        /// 断开连接
        /// </summary>
        event EventHandler<MqttClientDisconnectedEventArgs> OnDisconnected;

        /// <summary>
        /// 信息接收
        /// </summary>

        event EventHandler<MqttTopicReceivedEventArgs> OnReceived;

        /// <summary>
        /// 连接中事件
        /// </summary>
        event EventHandler<MqttConnectionEventArgs> OnConnecting;

        /// <summary>
        /// 连接
        /// </summary>
        Task ConnectAsync();


        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="content"></param>
        /// <param name="qos"></param>
        /// <returns></returns>
        Task PublishMessageAsync(string topic, string content, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="qos"></param>
        /// <returns></returns>
        Task SubscribeAsync(string topic, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce);

    }
}
