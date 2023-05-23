using SchoolManagement.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Services.Interface
{
    public interface IUserTokenService
    {
        Task<UserToken> CheckCodeRefreshToken(string codeRefreshToken);
        Task SaveToken(UserToken userToken);
    }
}
