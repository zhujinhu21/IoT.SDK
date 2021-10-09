using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiYi.IoT.SDK
{
    /// <summary>
    /// A publish service for publish a message to MQTT.
    /// </summary>
    public interface IIoTPublisher
    {
        IServiceProvider ServiceProvider { get; }


        Task PublishAsync<T>(string name, [NotNull] T contentObj, string callbackName = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronous publish an object message with custom headers
        /// </summary>
        /// <typeparam name="T">content object</typeparam>
        /// <param name="name">the topic name or exchange router key.</param>
        /// <param name="contentObj">message body content, that will be serialized. (can be null)</param>
        /// <param name="headers">message additional headers.</param>
        /// <param name="cancellationToken"></param>
        Task PublishAsync<T>(string name, [CanBeNull] T contentObj, IDictionary<string, string> headers, CancellationToken cancellationToken = default);

        /// <summary>
        /// Publish an object message.
        /// </summary>
        /// <param name="name">the topic name or exchange router key.</param>
        /// <param name="contentObj">message body content, that will be serialized. (can be null)</param>
        /// <param name="callbackName">callback subscriber name</param>
        void Publish<T>(string name, [CanBeNull] T contentObj, string callbackName = null);

        /// <summary>
        /// Publish an object message.
        /// </summary>
        /// <param name="name">the topic name or exchange router key.</param>
        /// <param name="contentObj">message body content, that will be serialized. (can be null)</param>
        /// <param name="headers">message additional headers.</param>
        void Publish<T>(string name, [CanBeNull] T contentObj, IDictionary<string, string> headers);
    }
}
