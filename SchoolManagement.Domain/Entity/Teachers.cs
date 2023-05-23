using SchoolManagement.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Entity
{
    public class Teachers : BaseEntity
    {
        public int Age { get; set; }
        public int Phone { get; set; }
        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public Classes Classes { get; set; }
    }
}
