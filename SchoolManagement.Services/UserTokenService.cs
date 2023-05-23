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
    public class UserTokenService : IUserTokenService
    {
        private readonly IBaseRepository<UserToken> _baseRepository;

        public UserTokenService(IBaseRepository<UserToken> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task SaveToken(UserToken userToken)
        {
            await _baseRepository.Create(userToken);
        }

        public async Task<UserToken> CheckCodeRefreshToken(string codeRefreshToken)
        {
            return await _baseRepository.GetSingleByConditionAsync(n => n.CodeRefreshToken== codeRefreshToken);
        }
    }
}
