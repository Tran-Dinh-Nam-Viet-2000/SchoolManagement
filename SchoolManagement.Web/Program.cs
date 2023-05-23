using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SchoolManagement.Data.Context;
using SchoolManagement.Data.Repositories;
using SchoolManagement.Infrastructure.Configuration;
using SchoolManagement.Services;
using SchoolManagement.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Gọi hàm từ project Infrastructure
ConfigurationService.RegisterContextDb(builder.Services, builder.Configuration);
ConfigurationService.RegisterDI(builder.Services);
ConfigurationTokenBear.RegisterJWT(builder.Services, builder.Configuration);
//Configure api and controller
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();