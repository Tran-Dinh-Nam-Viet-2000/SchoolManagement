using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Authentication.Service;
using SchoolManagement.Contracts.Dtos;
using SchoolManagement.Contracts.Models;
using SchoolManagement.Contracts.Requests;
using SchoolManagement.Domain.Entity;
using SchoolManagement.Services;
using SchoolManagement.Services.Interface;
using System.Reflection.Metadata.Ecma335;

namespace SchoolManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenHandler _tokenHandler;
        private readonly IUserTokenService _userTokenService;

        public AccountController(IUserService userService, ITokenHandler tokenHandler, IUserTokenService userTokenService)
        {
            _userService = userService;
            _tokenHandler = tokenHandler;
            _userTokenService = userTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return NotFound();
            }
            var user = await _userService.CheckLogin(loginRequest);

            if (user == null) return Unauthorized();

            //Tạo ra 2 token
            (string accessToken, DateTime expriredDateAccess) = await _tokenHandler.CreateAccessToken(user);
            (string codeRefreshToken, string refreshToken, DateTime expriredDateRefresh) = await _tokenHandler.CreateRefreshToken(user);

            //Lưu token vào db
            await _userTokenService.SaveToken(new UserToken
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                CodeRefreshToken= codeRefreshToken,
                ExpiredDateAccessToken = expriredDateAccess,
                ExpiredDateRefreshToken = expriredDateRefresh,
                CreatedDate = DateTime.Now,
                UserId = user.Id,
                Name = user.Name,
            });

            return Ok(new JwtModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Fullname = user.Name,
                Username = user.Username
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
        {
            return Ok(await _tokenHandler.ValidateRefreshToken(refreshTokenModel.RefreshToken));
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> Create(UserDto user)
        {
            if (user == null) return NotFound();
            return Ok(await _userService.Create(user));
        }
    }
}
