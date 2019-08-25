using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Common.Exceptions;
using Actio.Services.Activities.Services;
using Microsoft.Extensions.Logging;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Activities.Handlers
{
    public class CreateActivityHandler : ICommandHandler<CreateActivity>
    {
        private IBusClient _busClient;
        private readonly IActivityService _activityService;
        private readonly ILogger _logger;

        public CreateActivityHandler(IBusClient busClient, IActivityService activityService)
        {
            _busClient = busClient;
            _activityService = activityService;
            
        }
        public async Task HandleAsync(CreateActivity command)
        {
            Console.WriteLine($"Creating Activity: {command.Category} {command.Name}");
            try
            {
                await _activityService.AddAsync(command.Id, command.UserId, command.Category, command.Name, command.Description, command.CreatedAt);
                await _busClient.PublishAsync(new ActivityCreated(command.Id, command.UserId, command.Category, command.Name, command.Description, command.CreatedAt));
                return;
            }
            catch (ActioException ex)
            {
                await _busClient.PublishAsync(new CreatedActivityRejected(command.Id, ex.Code, ex.Message));
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                await _busClient.PublishAsync(new CreatedActivityRejected(command.Id, "error", ex.Message));
                Console.WriteLine(ex.Message);
            }
        }
    }
}
