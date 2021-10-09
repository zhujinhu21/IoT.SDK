using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DiYi.IoT.SDK.Diagnostics;
using DiYi.IoT.SDK.Messages;
using DiYi.IoT.SDK.Persistence;
using DiYi.IoT.SDK.Serialization;
using DiYi.IoT.SDK.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiYi.IoT.SDK.Internal
{
    internal class MessageSender : IMessageSender
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        private readonly IDataStorage _dataStorage;
        private readonly ISerializer _serializer;
        private readonly ITransport _transport;
        private readonly IOptions<IoTOptions> _options;

        // ReSharper disable once InconsistentNaming
        protected static readonly DiagnosticListener s_diagnosticListener =
            new DiagnosticListener(IoTDiagnosticListenerNames.DiagnosticListenerName);

        public MessageSender(
            ILogger<MessageSender> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            _options = serviceProvider.GetService<IOptions<IoTOptions>>();
            _dataStorage = serviceProvider.GetService<IDataStorage>();
            _serializer = serviceProvider.GetService<ISerializer>();
            _transport = serviceProvider.GetService<ITransport>();
        }

        public async Task<OperateResult> SendAsync(MediumMessage message)
        {
            bool retry;
            OperateResult result;
            do
            {
                var executedResult = await SendWithoutRetryAsync(message);
                result = executedResult.Item2;
                if (result == OperateResult.Success)
                {
                    return result;
                }
                retry = executedResult.Item1;
            } while (retry);

            return result;
        }

        private async Task<(bool, OperateResult)> SendWithoutRetryAsync(MediumMessage message)
        {
            var transportMsg = await _serializer.SerializeAsync(message.Origin);



            var result = await _transport.SendAsync(transportMsg);

            if (result.Succeeded)
            {
                //await SetSuccessfulState(message);

                return (false, OperateResult.Success);
            }
            else
            {
                //TracingError(tracingTimestamp, transportMsg, _transport.BrokerAddress, result);

                //var needRetry = await SetFailedState(message, result.Exception);

                return (false, OperateResult.Failed(result.Exception));
            }
        }



    }
}