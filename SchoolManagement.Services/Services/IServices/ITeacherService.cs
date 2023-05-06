using SchoolManagement.Contracts.Dtos;
using SchoolManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Services.Services.IServices
{
    public interface ITeacherService
    {
        IEnumerable<Teacher> GetAll();
        Task<Teacher> Create(TeacherDto teacher);
    }
}
