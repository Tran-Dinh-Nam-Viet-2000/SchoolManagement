using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Contracts.Dtos
{
    public class UserDto : BaseDto
    {
        [Required]
        [StringLength(150)]
        public string Username { get; set; }
        [Required]
        [StringLength(150)]
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
