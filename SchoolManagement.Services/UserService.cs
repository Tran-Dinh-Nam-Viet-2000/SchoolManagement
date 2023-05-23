using SchoolManagement.Contracts.Dtos;
using SchoolManagement.Contracts.Requests;
using SchoolManagement.Data.Entity;
using SchoolManagement.Data.Repositories;
using SchoolManagement.Domain.Entity;
using SchoolManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<Users> _userRepository;

        public UserService(IBaseRepository<Users> userRepository)
        {
            _userRepository = userRepository;
        }

        //Check login khi user nhập vào
        public async Task<Users> CheckLogin(LoginRequest accountRequest)
        {
            return await _userRepository.GetSingleByConditionAsync(n => n.Username == accountRequest.Username && n.Password == accountRequest.Password);
        }

        public async Task<Users> CheckUserName(string userName)
        {
            return await _userRepository.GetSingleByConditionAsync(n => n.Username == userName);
        }

        public async Task<Users> CheckUserById(int userId)
        {
            return await _userRepository.GetSingleByConditionAsync(n => n.Id== userId);
        }

        public async Task<Users> Create(UserDto user)
        {
            var createUser = new Users
            {
                Name = user.Name,
                Username = user.Username,
                Password = user.Password,
                CreatedDate = DateTime.Now,
            };
            await _userRepository.Create(createUser);
            return createUser;
        }
    }
}
 