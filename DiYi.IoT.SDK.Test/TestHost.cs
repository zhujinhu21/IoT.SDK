using System;
using System.Security.Cryptography.X509Certificates;
using DiYi.IoT.SDK.Mqtt;
using Microsoft.Extensions.DependencyInjection;

namespace DiYi.IoT.SDK.Test
{

    public abstract class TestHost : IDisposable
    {
        protected IServiceCollection _services;
        protected string ConnectionString;
        private IServiceProvider _provider;
        private IServiceProvider _scopedProvider;

        public TestHost()
        {
            CreateServiceCollection();
            PreBuildServices();
            BuildServices();
            PostBuildServices();
        }

        protected IServiceProvider Provider => _scopedProvider ?? _provider;

        private void CreateServiceCollection()
        {
            var services = new ServiceCollection();

            services.AddOptions();
            //services.AddLogging();

            ConnectionString = "";
            //var mqttConfig = new MqttConfig
            //{
            //    Server = "192.168.13.176",
            //    Port = 1883,
            //    ClientId = "MQTT:Cloud:Client:IOT:" + Guid.NewGuid().ToString("N"),
            //    Timeout = 30
            //};
            //services.AddSingleton(mqttConfig);

            services.AddIoT(x =>
            {

                x.UseMqtt(c =>
                {
                    c.HostName = "broker.emqx.io";
                    c.Port = 1883;
                    c.ClientId = "MQTT:Cloud:Client:IOT:" + Guid.NewGuid().ToString("N");
                    c.RedisConnectionStr = "localhost:6379";
                });
            });
            services.Configure<MqttMQOptions>(x =>
            {
                //x.ConnectionString = ConnectionString;
            });
            //services.AddSingleton<MySqlDataStorage>();
            //services.AddSingleton<IStorageInitializer, MySqlStorageInitializer>();
            //services.AddSingleton<ISerializer, JsonUtf8Serializer>();
            _services = services;
        }

        protected virtual void PreBuildServices()
        {
        }

        private void BuildServices()
        {
            _provider = _services.BuildServiceProvider();
        }

        protected virtual void PostBuildServices()
        {
        }

        public IDisposable CreateScope()
        {
            var scope = CreateScope(_provider);
            var loc = scope.ServiceProvider;
            _scopedProvider = loc;
            return new DelegateDisposable(() =>
            {
                if (_scopedProvider == loc)
                {
                    _scopedProvider = null;
                }
                scope.Dispose();
            });
        }

        public IServiceScope CreateScope(IServiceProvider provider)
        {
            var scope = provider.GetService<IServiceScopeFactory>().CreateScope();
            return scope;
        }

        public T GetService<T>() => Provider.GetService<T>();

        public virtual void Dispose()
        {
            (_provider as IDisposable)?.Dispose();
        }

        private class DelegateDisposable : IDisposable
        {
            private Action _dispose;

            public DelegateDisposable(Action dispose)
            {
                _dispose = dispose;
            }

            public void Dispose()
            {
                _dispose();
            }
        }
    }
}