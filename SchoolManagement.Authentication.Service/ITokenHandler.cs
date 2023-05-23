using Microsoft.AspNetCore.Authentication.JwtBearer;
using SchoolManagement.Contracts.Models;
using SchoolManagement.Domain.Entity;

namespace SchoolManagement.Authentication.Service
{
    public interface ITokenHandler
    {
        Task<(string, DateTime)> CreateAccessToken(Users users);
        Task<(string, string, DateTime)> CreateRefreshToken(Users users);
        Task<JwtModel> ValidateRefreshToken(string refreshToken);
        Task ValidateToken(TokenValidatedContext context);
    }
}