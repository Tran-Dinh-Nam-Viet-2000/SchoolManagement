using JWT.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Contracts.Models;
using SchoolManagement.Domain.Entity;
using SchoolManagement.Services;
using SchoolManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Authentication.Service
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IUserTokenService _userTokenService;

        public TokenHandler(IConfiguration configuration, IUserService userService, IUserTokenService userTokenService)
        {
            _userService = userService;
            _configuration = configuration;
            _userTokenService = userTokenService;
        }
        public async Task<(string, DateTime)> CreateAccessToken(Users users)
        {
            DateTime expriredToken = DateTime.Now.AddMinutes(3);

            var _claims = new Claim[]
            {
                //Generate 1 key chính cho token 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, _configuration["TokenAuthentication:Issuer"]),
                //Người cấp phát, sẽ là domain
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["TokenAuthentication:Issuer"], ClaimValueTypes.String, _configuration["TokenAuthentication:Issuer"]),
                //Thời gian cấp phát token
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToString(), ClaimValueTypes.DateTime, _configuration["TokenAuthentication:Issuer"]),
                //Tác giả chính của token
                new Claim(JwtRegisteredClaimNames.Aud, "SchoolManagement - Webapi", ClaimValueTypes.String, _configuration["TokenAuthentication:Issuer"]),
                //Thời hạn hết token, set thời gian
                new Claim(JwtRegisteredClaimNames.Exp, expriredToken.ToString(), ClaimValueTypes.String, _configuration["TokenAuthentication:Issuer"]),
                //Đăng ký thông tin người dùng
                new Claim(ClaimTypes.Name, users.Name, ClaimValueTypes.String, _configuration["TokenAuthentication:Issuer"]),
                new Claim("Username", users.Username, ClaimValueTypes.String, _configuration["TokenAuthentication:Issuer"]),
            };

            //Tạo 1 key cho token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenAuthentication:SigntureKey"]));
            //Mã hóa key
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Tạo thông tin cho token
            var tokenInfo = new JwtSecurityToken(
                issuer: _configuration["TokenAuthentication:Issuer"],
                audience: _configuration["TokenAuthentication:Audience"],
                claims: _claims,
                notBefore: DateTime.Now,
                expires: expriredToken,
                credential
            );

            //Khi có thông tin token rồi thì sẽ thực hiện viết nó ra đoạn mã
            string token = new JwtSecurityTokenHandler().WriteToken(tokenInfo);

            //FromResult là trả về kết quả dạng thread chứ ko trả về thread
            return await Task.FromResult((token, expriredToken));
        }

        public async Task<(string, string, DateTime)> CreateRefreshToken(Users users)
        {
            DateTime expriredToken = DateTime.Now.AddHours(1);
            string codeRefreshToken = Guid.NewGuid().ToString();
            var _claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, _configuration["TokenAuthentication:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["TokenAuthentication:Issuer"], ClaimValueTypes.String, _configuration["TokenAuthentication:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToString(), ClaimValueTypes.DateTime, _configuration["TokenAuthentication:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Exp, expriredToken.ToString(), ClaimValueTypes.String, _configuration["TokenAuthentication:Issuer"]),
                new Claim(ClaimTypes.SerialNumber, codeRefreshToken, ClaimValueTypes.String, _configuration["TokenAuthentication:Issuer"]),
                new Claim("Username", users.Username, ClaimValueTypes.String, _configuration["TokenAuthentication:Issuer"]),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenAuthentication:SigntureKey"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenInfo = new JwtSecurityToken(
                issuer: _configuration["TokenAuthentication:Issuer"],
                audience: _configuration["TokenAuthentication:Audience"],
                claims: _claims,
                notBefore: DateTime.Now,
                expires: expriredToken,
                credential
            );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenInfo);

            return await Task.FromResult((codeRefreshToken,token, expriredToken));
        }

        //Check validate của token, nếu thỏa mãn hết tất cả điều kiện thì sẽ hiển thị data
        public async Task ValidateToken(TokenValidatedContext context)
        {
            //Kiểm tra claims (thông tin người dùng)
            var claims = context.Principal.Claims.ToList();
            if (claims.Count == 0)
            {
                context.Fail("Token này không giống với thông tin của người dùng");
                return;
            }

            var identity = context.Principal.Identity as ClaimsIdentity;
            //Kiểm tra domain có đúng ko
            if (identity.FindFirst(JwtRegisteredClaimNames.Iss) == null)
            {
                context.Fail("Domain không đúng");
                return;
            }
            //Kiểm tra username của user
            if (identity.FindFirst("Username") == null)
            {
                string username = identity.FindFirst("Username").Value;
                var user = await _userService.CheckUserName(username);
                if (user == null) 
                {
                    context.Fail("Token không hợp lệ cho người dùng");
                    return;
                }
            }

            //Kiểm tra thời hạn của token đã hết hay còn
            if (identity.FindFirst(JwtRegisteredClaimNames.Exp) != null)
            {
                var dateExp = identity.FindFirst(JwtRegisteredClaimNames.Exp).Value;
                long ticks = long.Parse(dateExp);
                var date = DateTimeOffset.FromUnixTimeSeconds(ticks).DateTime;
                var minutes = date.Subtract(DateTime.Now).TotalMinutes;
                if (minutes < 0)
                {
                    context.Fail("Token đã hết hạn.");
                    return;
                }
            }
        }

        public async Task<JwtModel> ValidateRefreshToken(string refreshToken)
        {
            var claimPriciple = new JwtSecurityTokenHandler().ValidateToken(
                refreshToken,
                new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenAuthentication:SigntureKey"])),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidIssuer = _configuration["TokenAuthentication:Issuer"],
                    ValidateAudience = false,
                    ValidAudience = _configuration["TokenAuthentication:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);
            if (claimPriciple == null) return new();

            string serialNumber = claimPriciple.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value;
            if (string.IsNullOrEmpty(serialNumber)) return new();

            //Nếu codeRefreshToken trong db đã tồn tại thì tạo mới accesstoken và refreshtoken gửi lên cho client
            UserToken userToken = await _userTokenService.CheckCodeRefreshToken(serialNumber);
            if (userToken != null)
            {
                //Lấy lại thông tin user xong tạo mới access và refreshtoken
                Users users = await _userService.CheckUserById(userToken.UserId);
                (string newAccessToken, DateTime createdDate) = await CreateAccessToken(users);
                (string codeRefreshToken, string newRefreshToken, DateTime newCreatedDate) = await CreateRefreshToken(users);

                return new JwtModel
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    Fullname = users.Name,
                    Username = users.Username
                };
            }
            return new();
        }
    }
}
