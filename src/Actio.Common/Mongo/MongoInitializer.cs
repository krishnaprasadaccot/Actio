using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Actio.Common.Mongo
{
    
    public class MongoInitializer:IDatabaseInitializer
    {
        private bool _initialized;
        private readonly bool _seed;
        private readonly IMongoDatabase _database;
        private readonly IDatabaseSeeder _seeder;
        public MongoInitializer(IMongoDatabase db,IDatabaseSeeder seeder,IOptions<MongoOptions> opts)
        {
            _database = db;
            _seed = opts.Value.Seed;
            _seeder = seeder;

        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;
            RegisterConventions();
            _initialized = true;
            if(!_seed)
            {
                return;
            }
            await _seeder.SeedAsync();
        }

        private void RegisterConventions()
        {
            ConventionRegistry.Register("ActioConventions", new MongoConvention(), x => true);
        }

        private class MongoConvention : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}
