using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class AppUser : IdentityUser
    {
 
        public string? Name { get; set; }
       
    }
}
