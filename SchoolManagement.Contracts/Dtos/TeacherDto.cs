using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Contracts.Dtos
{
    public  class TeacherDto : BaseDto
    {
        public int Age { get; set; }
        public int Phone { get; set; }
        public int ClassId { get; set; }
    }
}
