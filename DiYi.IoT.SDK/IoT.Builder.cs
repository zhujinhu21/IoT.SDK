using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace DiYi.IoT.SDK
{
    public sealed class IoTBuilder
    {
        public IoTBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Gets the <see cref="IServiceCollection" /> where MVC services are configured.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Add an <see cref="IIoTPublisher" />.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        public IoTBuilder AddProducerService<T>()
            where T : class, IIoTPublisher
        {
            return AddScoped(typeof(IIoTPublisher), typeof(T));
        }

        /// <summary>
        /// Adds a scoped service of the type specified in serviceType with an implementation
        /// </summary>
        private IoTBuilder AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in serviceType with an implementation
        /// </summary>
        private IoTBuilder AddSingleton(Type serviceType, Type concreteType)
        {
            Services.AddSingleton(serviceType, concreteType);
            return this;
        }
    }
}
