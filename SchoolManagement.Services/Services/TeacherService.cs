using SchoolManagement.Contracts.Dtos;
using SchoolManagement.Data.Entity;
using SchoolManagement.Data.Repositories;
using SchoolManagement.Services.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Services.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IBaseRepository<Teacher> _repository;

        public TeacherService(IBaseRepository<Teacher> repository) 
        {
            _repository = repository;
        }
        public async Task<Teacher> Create(TeacherDto teacher)
        {
            var create = new Teacher() {
                Name= teacher.Name,
                Age= teacher.Age,
                Phone = teacher.Phone
            };
            await _repository.Create(create);
            return create;
        }

        public IEnumerable<Teacher> GetAll()
        {
             return _repository.GetAll();
        }
    }
}
