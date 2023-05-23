using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagement.Authentication.Service;
using SchoolManagement.Data.Context;
using SchoolManagement.Data.Repositories;
using SchoolManagement.Services;
using SchoolManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Infrastructure.Configuration
{
    public static class ConfigurationService
    {
        //Tiêm (gọi) những hàm trong class này cho file Startup.cs, tách ra dễ quản lý

        //Đăng ký Db
        public static void RegisterContextDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options
                                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        //Đăng ký DependencyInjection
        public static void RegisterDI(this IServiceCollection services)
        {
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IUserTokenService, UserTokenService>();
        }
    }
}
