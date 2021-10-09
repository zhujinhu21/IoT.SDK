using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DiYi.IoT.SDK.Util;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Protocol;
using NewLife.Caching;
using NewLife.Log;

namespace DiYi.IoT.SDK.Mqtt
{
    /// <summary>
    /// Mqtt处理者
    /// </summary>
    public class MqttHandler : IMqttHandler
    {
        public readonly IMqttNetClient MqttNetClient;
        private readonly FullRedis _redis; //老redis，柜子token是写在旧redis地址上的 

        public MqttHandler(IMqttNetClient mqttClient, FullRedis redis)
        {
            MqttNetClient = mqttClient;
            MqttNetClient.OnConnected += MqttNetClient_OnConnected;
            MqttNetClient.OnDisconnected += MqttNetClient_OnDisconnected;
            MqttNetClient.OnReceived += MqttNetClient_OnReceived;
            MqttNetClient.OnConnecting += MqttClient_OnConnecting;
            MqttNetClient.ConnectAsync();

            _redis = redis;
        }
        public async Task PublishOpenCellMessage(bool isAsync = false, int timeoutSeconds = 2)
        {
            await MqttNetClient.PublishMessageAsync("test", "123456", MqttQualityOfServiceLevel.AtLeastOnce);
        }

        /// <summary>
        ///IoT 订阅
        /// </summary>
        private void Subscribe()
        {
            var receiveTopic = MqttNetClient.GetClintId() + "/IoTBase";
            MqttNetClient.SubscribeAsync(receiveTopic);
        }

        /// <summary>
        /// MQTT数据接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttNetClient_OnReceived(Object sender, MqttTopicReceivedEventArgs e)
        {
            var payload = EncryptUtil.Base64Decode(e.Message);
            XTrace.WriteLine("收到mqtt消息：" + payload);

        }

        /// <summary>
        /// 连接成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttNetClient_OnConnected(Object sender, MqttClientConnectedEventArgs e)
        {
            Console.WriteLine("连接成功");
            Subscribe();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttNetClient_OnDisconnected(Object sender, MqttClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"Mqtt>>Disconnected【{ MqttNetClient.GetClintId() }】>>已断开连接");

            MqttNetClient.ConnectAsync();
        }

        /// <summary>
        /// 连接中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_OnConnecting(Object sender, MqttConnectionEventArgs e) => Console.WriteLine($"Mqtt>>Reconnecting【{ MqttNetClient.GetClintId() }】>>重连次数{e.ReconnectingCount},{(e.ReconnectingException == null ? String.Empty : "异常：" + e.ReconnectingException.Message)}");

        /// <summary>
        /// 基于递易通用标准 简单加密数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private String EncryptData(String data)
        {
            var dataLength = Encoding.Default.GetByteCount(data);
            return "DyIot#" + dataLength.ToString().PadLeft(4, '0') + data;
        }
    }
}
