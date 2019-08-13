using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Activities.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly IMongoDatabase _database;
        public ActivityRepository(IMongoDatabase db)
        {
            _database = db;
        }
        public async Task AddAsync(Activity activity)
            => await Collection.InsertOneAsync(activity);

        public async Task<Activity> GetAsync(Guid id) => await Collection.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);

        private IMongoCollection<Activity> Collection => _database.GetCollection<Activity>("Activities");
    }
}
