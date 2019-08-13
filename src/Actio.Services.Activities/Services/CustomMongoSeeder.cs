using Actio.Common.Mongo;
using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Activities.Services
{
    public class CustomMongoSeeder:MongoSeeder
    {
        private readonly ICategoryRepository _categoryRepository;
        public CustomMongoSeeder(IMongoDatabase db, ICategoryRepository categoryRepository):base(db)
        {
            _categoryRepository = categoryRepository;
        }
        protected override async Task CustomSeedAsync()
        {
            var categories = new List<string>
            {
                "work",
                "sport",
                "hobby"
            };
            await Task.WhenAll(categories.Select(x => _categoryRepository.AddAsync(new Category(x))));
        }
    }
}
