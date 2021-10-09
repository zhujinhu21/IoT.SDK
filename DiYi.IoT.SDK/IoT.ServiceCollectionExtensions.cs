using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using DiYi.IoT.SDK.Internal;
using DiYi.IoT.SDK.Processor;
using DiYi.IoT.SDK.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NewLife.Caching;
using DiYi.IoT.SDK.Transport;

namespace DiYi.IoT.SDK
{
    public static class ServiceCollectionExtensions
    {
        internal static  IServiceCollection ServiceCollection;

        public static IoTBuilder AddIoT(this IServiceCollection services, Action<IoTOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }
            ServiceCollection = services;

            services.TryAddSingleton<IIoTPublisher, IoTPublisher>();


            //Sender and Executors   
            services.TryAddSingleton<IMessageSender, MessageSender>();
            services.TryAddSingleton<IDispatcher, Dispatcher>();

            services.TryAddSingleton<ISerializer, JsonUtf8Serializer>();

            services.TryAddSingleton<ISubscribeDispatcher, SubscribeDispatcher>();

            //Options and extension service
            var options = new IoTOptions();
            setupAction(options);
            foreach (var serviceExtension in options.Extensions)
            {
                serviceExtension.AddServices(services);
            }

            services.Configure(setupAction);

            return new IoTBuilder(services);
        }
    }
}
