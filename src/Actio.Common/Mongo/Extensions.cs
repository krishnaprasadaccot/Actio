using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Mongo
{
    public static class Extensions
    {
        public static void AddMongoDB(this IServiceCollection service, IConfiguration configuration)
        {
            service.Configure<MongoOptions>(configuration.GetSection("mongo"));
            service.AddSingleton<MongoClient>(c =>
            {
                var options = c.GetService<IOptions<MongoOptions>>();
                return new MongoClient(options.Value.ConnectionString);
            });

            service.AddTransient<IMongoDatabase>(c =>
            {
                var options = c.GetService<IOptions<MongoOptions>>();
                var client = c.GetService<MongoClient>();

                return client.GetDatabase(options.Value.Database);
            });
            service.AddTransient<IDatabaseInitializer, MongoInitializer>();
            service.AddTransient<IDatabaseSeeder, MongoSeeder>();
        }
    }
}
