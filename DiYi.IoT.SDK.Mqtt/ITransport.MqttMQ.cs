using System;
using System.Linq;
using System.Threading.Tasks;
using DiYi.IoT.SDK.Internal;
using DiYi.IoT.SDK.Messages;
using DiYi.IoT.SDK.Transport;
using Microsoft.Extensions.Logging;


namespace DiYi.IoT.SDK.Mqtt
{
    internal sealed class MqttMQTransport : ITransport
    {

        private readonly ILogger _logger;
        private readonly IMqttHandler _mqttHandler;

        public MqttMQTransport(ILogger<MqttMQTransport> logger, IMqttHandler mqttHandler)
        {
            _logger = logger;
            _mqttHandler = mqttHandler;
        }

        //public BrokerAddress BrokerAddress => new BrokerAddress("MqttMq", _connectionChannelPool.HostAddress);

        public Task<OperateResult> SendAsync(TransportMessage message)
        {
            //IModel channel = null;
            try
            {
                //channel = _connectionChannelPool.Rent();

                //channel.ConfirmSelect();

                //var props = channel.CreateBasicProperties();
                //props.DeliveryMode = 2;
                //props.Headers = message.Headers.ToDictionary(x => x.Key, x => (object) x.Value);

                //channel.ExchangeDeclare(_exchange, RabbitMQOptions.ExchangeType, true);

                //channel.BasicPublish(_exchange, message.GetName(), props, message.Body);

                //channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(5));

                _mqttHandler.PublishOpenCellMessage();
                _logger.LogDebug($"MqttMq topic message [{message.GetName()}] has been published.");

                return Task.FromResult(OperateResult.Success);
            }
            catch (Exception ex)
            {
                var wrapperEx = new PublisherSentFailedException(ex.Message, ex);
                var errors = new OperateError
                {
                    Code = ex.HResult.ToString(),
                    Description = ex.Message
                };

                return Task.FromResult(OperateResult.Failed(wrapperEx, errors));
            }
            finally
            {
                //if (channel != null)
                //{
                //    var returned = _connectionChannelPool.Return(channel);
                //    if (!returned)
                //    {
                //        channel.Dispose();
                //    }
                //}
            }
        }
    }
}