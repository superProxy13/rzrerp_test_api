using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain
{
    public class UpdateUserModel
    {
        public long Id { get; set; }
        public short RoleId { get; set; }
        public short CompanyID { get; set; }
    }
}
