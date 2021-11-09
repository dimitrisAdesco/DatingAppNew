using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class LoginDto
    {

        [Required]
        
        public string Username { get; set; }

        [Required]
        [StringLength(225)]
        public string Password { get; set; }

    }
}