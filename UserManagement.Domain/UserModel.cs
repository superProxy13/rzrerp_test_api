using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserManagement.Domain;

namespace UserManagement.API.Models
{
    public class UserModel : LoginModel
    {
        public long Id { get; set; }
        public short RoleId { get; set; }
        public short CompanyID { get; set; }
        public DateTime DateUpdated  {get;set;}
    }
}
