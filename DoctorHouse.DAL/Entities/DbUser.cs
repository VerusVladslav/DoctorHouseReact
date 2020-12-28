using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DoctorHouse.DAL.Entities
{
    public class DbUser : IdentityUser<long>
    {

        public virtual ICollection<DbUserRole> UserRoles { get; set; }
    }
}
