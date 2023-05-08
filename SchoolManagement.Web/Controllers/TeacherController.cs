using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Contracts.Dtos;
using SchoolManagement.Data.Entity;
using SchoolManagement.Services.Services;
using SchoolManagement.Services.Services.IServices;
using System.Collections;

namespace SchoolManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("Search")]
        public async Task<IEnumerable<Teacher>> Search(string keyword)
        {
             return await _teacherService.Search(keyword);
        }

        [HttpGet]
        public IEnumerable<Teacher> GetAll()
        {
            return _teacherService.GetAll();
        }

        [HttpPost("Create")]
        public async Task<Teacher> Create(TeacherDto teacher)
        {
            return  await _teacherService.Create(teacher);
        }
    }
}
