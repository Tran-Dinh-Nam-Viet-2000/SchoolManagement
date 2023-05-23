using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Contracts.Dtos;
using SchoolManagement.Data.Entity;
using SchoolManagement.Services.Interface;
using System.Collections;

namespace SchoolManagement.Web.Controllers
{
    [Authorize]
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
        public async Task<IEnumerable<Teachers>> Search(string keyword)
        {
             return await _teacherService.Search(keyword);
        }

        [HttpGet("Get-all")]
        public async Task<IEnumerable<Teachers>> GetAll()
        {
            return await _teacherService.GetAll();
        }

        [HttpGet("Get-data-by-condition")]
        public async Task<IEnumerable<Teachers>> GetDataByCondition()
        {
            return await _teacherService.GetDataByCondition();
        }

        [HttpPost("Create")]
        public async Task<Teachers> Create(TeacherDto teacher)
        {
            return await _teacherService.Create(teacher);
        }

        [HttpPut("Update")]
        public async Task<Teachers> Update(TeacherDto teacher, int id)
        {
            return await _teacherService.Update(teacher,id);
        }

        [HttpDelete("Delete")]
        public void Delete(int id)
        {
            _teacherService.Delete(id);
        }
    }
}
