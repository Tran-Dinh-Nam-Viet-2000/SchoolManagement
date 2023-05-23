using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entity
{
    [Table("Account")]
    public class Users : BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string Username { get; set; }
        [Required]
        [StringLength(150)]
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set;}
    }
}
