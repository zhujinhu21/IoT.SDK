using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using NewLife.Log;

namespace DiYi.IoT.SDK.Mqtt
{
    public class MqttNetClient : IMqttNetClient
    {
        public IMqttClient MqttClient { get; }
        private IMqttClientOptions _options;
        private MqttConfig _mqttConfig;
        private static readonly object _lock = new object();

        /// <summary>
        /// 链接次数
        /// </summary>
        private long _connectingCount;

        /// <summary>
        /// 是否链接中
        /// </summary>
        private bool _isConnecting = false;

        /// <summary>
        /// 连接成功 
        /// </summary>
        public event EventHandler<MqttClientConnectedEventArgs> OnConnected;

        /// <summary>
        /// 断开连接
        /// </summary>
        public event EventHandler<MqttClientDisconnectedEventArgs> OnDisconnected;

        /// <summary>
        /// 信息接收
        /// </summary>
        public event EventHandler<MqttTopicReceivedEventArgs> OnReceived;

        /// <summary>
        /// 重连
        /// </summary>
        public event EventHandler<MqttConnectionEventArgs> OnConnecting;

        /// <summary>
        /// 实例化 
        /// </summary>
        /// <param name="mqttConfig"></param>
        public MqttNetClient(MqttConfig mqttConfig)
        {
            _mqttConfig = mqttConfig;
            var factory = new MqttFactory();
            MqttClient = factory.CreateMqttClient();

            //实例化一个MqttClientOptionsBulider
            _options = new MqttClientOptionsBuilder()
                .WithTcpServer(_mqttConfig.Server, _mqttConfig.Port)
                .WithCredentials(_mqttConfig.Username, _mqttConfig.Password)
                .WithClientId(_mqttConfig.ClientId)
                .Build();


            //是客户端连接成功时触发的事件

            //是客户端连接成功时触发的事件
            MqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(MqttClient_Connected);

            //是客户端断开连接时触发的事件
            MqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(MqttClient_Disconnected);

            //是服务器接收到消息时触发的事件，可用来响应特定消息
            MqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(MqttClient_ApplicationMessageReceived);

        }


        /// <summary>
        /// 连接成功
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private void MqttClient_Connected(MqttClientConnectedEventArgs e)
        {
            //连接重新订阅
            if (_mqttConfig.SubscribeTopicList != null)
            {
                foreach (var item in _mqttConfig.SubscribeTopicList)
                {
                    MqttClient.SubscribeAsync(item.Key, item.Value);
                }
            }

            OnConnected?.Invoke(this, e);
        }

        /// <summary>
        /// 信息接收
        /// </summary>
        /// <param name="e"></param>
        private void MqttClient_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            OnReceived?.Invoke(this, new MqttTopicReceivedEventArgs(topic, payload));
        }


        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private void MqttClient_Disconnected(MqttClientDisconnectedEventArgs e)
        {
            if (OnDisconnected != null)
            {
                OnDisconnected(this, e);
            }
        }


        /// <summary>
        /// 连接
        /// </summary>
        public async Task ConnectAsync()
        {
            lock (_lock)
            {
                if (_isConnecting)
                {
                    return;
                }
                _isConnecting = true;
            }
            await Task.Factory.StartNew(async () =>
            {
                while (!MqttClient.IsConnected)
                {
                    Exception reconnectingEx = null;
                    _connectingCount++;
                    try
                    {
                        var result = await MqttClient.ConnectAsync(_options);
                    }
                    catch (Exception ex)
                    {
                        reconnectingEx = ex;
                        XTrace.WriteException(ex);
                    }
                    if (OnConnecting != null)
                    {
                        OnConnecting(this, new MqttConnectionEventArgs(_connectingCount, reconnectingEx));
                    }
                    await Task.Delay(3000);
                }
                _isConnecting = false;
                _connectingCount = 0;
            });

        }

        /// <summary>        
        /// 发送消息
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="content"></param>
        /// <param name="qos"></param>
        /// <returns></returns>
        public async Task PublishMessageAsync(string topic, string content, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
        {
            var message = new MqttApplicationMessageBuilder();
            message.WithTopic(topic);
            message.WithPayload(content);

            switch (qos)
            {
                case MqttQualityOfServiceLevel.AtMostOnce:
                    message.WithAtMostOnceQoS();
                    break;
                case MqttQualityOfServiceLevel.AtLeastOnce:
                    message.WithAtLeastOnceQoS();
                    break;
                case MqttQualityOfServiceLevel.ExactlyOnce:
                    message.WithExactlyOnceQoS();
                    break;
                default:
                    break;
            }

            message.WithRetainFlag(false);
            await MqttClient.PublishAsync(message.Build());
        }


        /// <summary>
        /// 订阅Topic
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="qos"></param>
        public async Task SubscribeAsync(string topic, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
        {
            _mqttConfig.SubscribeTopicList ??= new Dictionary<string, MqttQualityOfServiceLevel>();
            if (_mqttConfig.SubscribeTopicList.ContainsKey(topic))
            {
                return;
            }
            var isAdded = false;
            lock (_lock)
            {
                if (!_mqttConfig.SubscribeTopicList.ContainsKey(topic))
                {
                    _mqttConfig.SubscribeTopicList.Add(topic, qos);
                    isAdded = true;
                }
            }
            if (isAdded)
            {
                await MqttClient.SubscribeAsync(topic, qos);
            }
        }

        /// <summary>
        /// 获取连接Id
        /// </summary>
        /// <returns></returns>
        public string GetClintId()
        {
            return _mqttConfig.ClientId;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        public MqttConfig GetMqttConfig()
        {
            return _mqttConfig;
        }
    }
}
