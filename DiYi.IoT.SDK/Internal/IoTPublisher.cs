using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using DiYi.IoT.SDK.Diagnostics;
using DiYi.IoT.SDK.Messages;
using DiYi.IoT.SDK.Persistence;
using DiYi.IoT.SDK.Transport;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.Client;
using NewLife.Data;

namespace DiYi.IoT.SDK.Internal
{
    internal class IoTPublisher : IIoTPublisher
    {
        private readonly IDispatcher _dispatcher;
        private readonly IDataStorage _storage;

        // ReSharper disable once InconsistentNaming
        protected static readonly DiagnosticListener s_diagnosticListener =
            new DiagnosticListener(IoTDiagnosticListenerNames.DiagnosticListenerName);

        public IoTPublisher(IServiceProvider service)
        {
            ServiceProvider = service;
            _dispatcher = service.GetRequiredService<IDispatcher>();
            //_storage = service.GetRequiredService<IDataStorage>();
            //Transaction = new AsyncLocal<ICapTransaction>();
        }

        public IServiceProvider ServiceProvider { get; }

        //public AsyncLocal<ICapTransaction> Transaction { get; }

        public Task PublishAsync<T>(string name, T value, IDictionary<string, string> headers, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => Publish(name, value, headers), cancellationToken);
        }

        public Task PublishAsync<T>(string name, T value, string callbackName = null,
            CancellationToken cancellationToken = default)
        {
            return Task.Run(() => Publish(name, value, callbackName), cancellationToken);
        }

        public void Publish<T>(string name, T value, string callbackName = null)
        {
            var header = new Dictionary<string, string>
            {
                {Headers.CallbackName, callbackName}
            };

            Publish(name, value, header);
        }

        public void Publish<T>(string name, T value, IDictionary<string, string> headers)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            headers ??= new Dictionary<string, string>();

            if (!headers.ContainsKey(Headers.MessageId))
            {
                var messageId = new Snowflake().NewId(DateTime.Now).ToString();
                headers.Add(Headers.MessageId, messageId);
            }

            if (!headers.ContainsKey(Headers.CorrelationId))
            {
                headers.Add(Headers.CorrelationId, headers[Headers.MessageId]);
                headers.Add(Headers.CorrelationSequence, 0.ToString());
            }
            headers.Add(Headers.MessageName, name);
            headers.Add(Headers.Type, typeof(T).Name);
            headers.Add(Headers.SentTime, DateTimeOffset.Now.ToString());

            var message = new Message(headers, value);

            long? tracingTimestamp = null;
            try
            {
                tracingTimestamp = TracingBefore(message);

                //var mediumMessage = _storage.StoreMessage(name, message);

                var mediumMessage = new MediumMessage
                {
                    DbId = message.GetId(),
                    Origin = message,
                    Content = JsonSerializer.Serialize(message),
                    Added = DateTime.Now,
                    ExpiresAt = null,
                    Retries = 0
                };
                TracingAfter(tracingTimestamp, message);

                _dispatcher.EnqueueToPublish(mediumMessage);

            }
            catch (Exception e)
            {
                TracingError(tracingTimestamp, message, e);

                throw;
            }
        }

        #region tracing

        private long? TracingBefore(Message message)
        {
            if (s_diagnosticListener.IsEnabled(IoTDiagnosticListenerNames.BeforePublishMessageStore))
            {
                var eventData = new IoTEventDataPubStore()
                {
                    OperationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    Operation = message.GetName(),
                    Message = message
                };

                s_diagnosticListener.Write(IoTDiagnosticListenerNames.BeforePublishMessageStore, eventData);

                return eventData.OperationTimestamp;
            }

            return null;
        }

        private void TracingAfter(long? tracingTimestamp, Message message)
        {
            if (tracingTimestamp != null && s_diagnosticListener.IsEnabled(IoTDiagnosticListenerNames.AfterPublishMessageStore))
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var eventData = new IoTEventDataPubStore()
                {
                    OperationTimestamp = now,
                    Operation = message.GetName(),
                    Message = message,
                    ElapsedTimeMs = now - tracingTimestamp.Value
                };

                s_diagnosticListener.Write(IoTDiagnosticListenerNames.AfterPublishMessageStore, eventData);
            }
        }

        private void TracingError(long? tracingTimestamp, Message message, Exception ex)
        {
            if (tracingTimestamp != null && s_diagnosticListener.IsEnabled(IoTDiagnosticListenerNames.ErrorPublishMessageStore))
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var eventData = new IoTEventDataPubStore()
                {
                    OperationTimestamp = now,
                    Operation = message.GetName(),
                    Message = message,
                    ElapsedTimeMs = now - tracingTimestamp.Value,
                    Exception = ex
                };

                s_diagnosticListener.Write(IoTDiagnosticListenerNames.ErrorPublishMessageStore, eventData);
            }
        }

        #endregion
    }
}
