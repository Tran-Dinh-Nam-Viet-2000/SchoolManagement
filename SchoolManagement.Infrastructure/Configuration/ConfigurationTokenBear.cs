using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Authentication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Configuration
{
    public static class ConfigurationTokenBear
    {
        public static void RegisterJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenAuthentication:SigntureKey"])),

                     ValidateIssuer = false,
                     ValidIssuer = configuration["TokenAuthentication:Issuer"],

                     ValidateAudience = false,
                     ValidAudience = configuration["TokenAuthentication:Audience"],

                     ValidateLifetime = true,
                };
                //Check validate token
                //Đầu tiên luồng sẽ chạy vào OnMessageReceived xong đến OnTokenValidated để check các điều kiện, nếu ok thì chạy xuống OnChallenge
                //Nếu lỗi code internal (nội bộ) thì sẽ chạy vào OnAuthenticationFailed
                options.Events = new JwtBearerEvents()
                {
                     OnTokenValidated = context =>
                     {
                         var tokenHandler = context.HttpContext.RequestServices.GetRequiredService<ITokenHandler>();
                         return tokenHandler.ValidateToken(context);
                     },
                     OnMessageReceived = context =>
                     {
                          return Task.CompletedTask;
                     },
                     OnAuthenticationFailed = context =>
                     {
                          return Task.CompletedTask;
                     },
                     OnChallenge = context =>
                     {
                          return Task.CompletedTask;
                     },
                };
            });
        }
    }
}
