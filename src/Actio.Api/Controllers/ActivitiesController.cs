using System;
using System.Linq;
using System.Threading.Tasks;
using Actio.Api.Repositories;
using Actio.Common.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;

namespace Actio.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActivitiesController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly IActivityRepository _activityRepository;

        public ActivitiesController(IBusClient busClient, IActivityRepository activityRepository)
        {
            _busClient = busClient;
            _activityRepository = activityRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateActivity command)
        {
            command.Id = Guid.NewGuid();
            command.CreatedAt = DateTime.UtcNow;
            command.UserId = Guid.Parse(User.Identity.Name);
            await _busClient.PublishAsync(command);

            return Accepted($"activities/{command.Id}");
        }
        [HttpGet("")]

        public async Task<IActionResult> Get()
        {
            var activities = await _activityRepository.BrowseAsync(Guid.Parse(User.Identity.Name));

            return Json(activities.Select(x => new { x.Id, x.Name, x.Category, x.CreatedAt }));
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> Get(Guid id)
        {
            var activitiy = await _activityRepository.GetAsync(id);
            if(activitiy ==null)
            {
                return NotFound();
            }
            if(activitiy.UserId != Guid.Parse(User.Identity.Name))
            {
                return Unauthorized();
            }
            return Json(activitiy);
        }
    }
}