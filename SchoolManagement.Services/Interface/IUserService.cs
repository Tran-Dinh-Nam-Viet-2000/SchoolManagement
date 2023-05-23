using SchoolManagement.Contracts.Dtos;
using SchoolManagement.Contracts.Requests;
using SchoolManagement.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Services.Interface
{
    public interface IUserService
    {
        Task<Users> CheckLogin(LoginRequest accountRequest);
        Task<Users> CheckUserById(int id);
        Task<Users> CheckUserName(string userName);
        Task<Users> Create(UserDto user);
    }
}
