using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain
{
    public class NewUserModel : LoginModel
    {
        public short RoleId { get; set; }
        public short CompanyID { get; set; }
    }
}
