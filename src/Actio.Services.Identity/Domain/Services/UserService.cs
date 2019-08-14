using Actio.Common.Exceptions;
using Actio.Services.Identity.Domain.Models;
using Actio.Services.Identity.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;

        public UserService(IUserRepository userRepository, IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
        }
        public async Task LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if(user==null)
            {
                throw new ActioException("invalid_creadentials", $"Invalid Creadentails Entered : {email}");
            }
            if(!user.ValidatePassword(password,_encrypter))
            {
                throw new ActioException("invalid_creadentials", $"Invalid Password Entered for username: {email}");
            }
        }

        public async Task RegisterAsync(string email, string password, string name)
        {
            var user = await _userRepository.GetAsync(email);
            if(user!= null)
            {
                throw new ActioException("email_in_use", $"Email:{email} is already used.");
            }
            user = new User(email, name);
            user.SetPassword(password, _encrypter);
            await _userRepository.AddAsync(user);
        }
    }
}
