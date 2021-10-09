using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DiYi.IoT.SDK.Mqtt
{
    public class RedisOptions
    {
        /// <summary>
        /// Gets or sets the database's connection string that will be used to store database entities.
        /// </summary>
        public string ConnectionString { get; set; }
    }

    internal class ConfigureRedisOptions : IConfigureOptions<RedisOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ConfigureRedisOptions(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(RedisOptions options)
        {
            
            using var scope = _serviceScopeFactory.CreateScope();
            var provider = scope.ServiceProvider;
            //using var dbContext = (DbContext)provider.GetRequiredService(options.DbContextType);
            //options.ConnectionString = dbContext.Database.GetDbConnection().ConnectionString;
        }
    }

    internal class ConfigureMqttOptions : IConfigureOptions<MqttMQOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ConfigureMqttOptions(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(MqttMQOptions options)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var provider = scope.ServiceProvider;
            var opt=provider.GetRequiredService(typeof(MqttMQOptions));
            //using var dbContext = (DbContext)provider.GetRequiredService(options.DbContextType);
            //options.ConnectionString = dbContext.Database.GetDbConnection().ConnectionString;
        }
    }
}
