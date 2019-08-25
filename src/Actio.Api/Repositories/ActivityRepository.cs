using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Actio.Api.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Activity = Actio.Api.Models.Activity;

namespace Actio.Api.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly IMongoDatabase _database;
        public ActivityRepository(IMongoDatabase db)
        {
            _database = db;
        }
        public async Task AddAsync(Activity activity)=>await Collection.InsertOneAsync(activity);

        public async Task<IEnumerable<Models.Activity>> BrowseAsync(Guid userId)
            => await Collection
                .AsQueryable()
                .Where(x => x.UserId == userId).ToListAsync();

        public async Task<Activity> GetAsync(Guid id)=>await Collection.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);

        private IMongoCollection<Activity> Collection => _database.GetCollection<Activity>("Activities");
    }
}
