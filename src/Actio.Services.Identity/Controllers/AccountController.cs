using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actio.Services.Identity.Domain.Services;
using Actio.Common.Commands;

namespace Actio.Services.Identity.Controllers
{
    [Route("")]
    public class AccountController: Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticatedUser command)
            => Json(await _userService.LoginAsync(command.Email, command.Password));
    }
}
