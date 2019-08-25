using System;
using System.Threading.Tasks;
using Actio.Api.Repositories;
using Actio.Common.Events;

namespace Actio.Api.Handlers
{
    public class ActivityCreatedHandler : IEventHandler<ActivityCreated>
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityCreatedHandler(IActivityRepository activityRepository)
        {
            Console.WriteLine("EventHandler--------------------------");
            _activityRepository = activityRepository;
        }
        public async Task HandleAsync(ActivityCreated @event)
        {
            Console.WriteLine("EventHandler--------------------------");
            await _activityRepository.AddAsync(new Models.Activity {
                Id=@event.Id,
                UserId = @event.UserId,
                Name = @event.Name,

            });
            Console.WriteLine($"Activy Created: {@event.Name}");
        }
    }
}
