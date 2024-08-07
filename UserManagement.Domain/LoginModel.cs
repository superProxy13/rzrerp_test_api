using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.API.Models;

namespace UserManagement.Domain
{
    public  class LoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
