﻿using Actio.Common.Exceptions;
using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Activities.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ActivityService(IActivityRepository activityRepository, ICategoryRepository categoryRepository)
        {
            _activityRepository = activityRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task AddAsync(Guid id, Guid userId, string category, string name, string description, DateTime createdDate)
        {
            var activityCategory = await _categoryRepository.GetAsync(category);
            if(activityCategory ==null)
            {
                throw new ActioException("Category not found", $"Category:{category} was not found.");
            }
            var activity = new Activity(id, activityCategory, userId, name, description, createdDate);
            await _activityRepository.AddAsync(activity);
        }
    }
}
