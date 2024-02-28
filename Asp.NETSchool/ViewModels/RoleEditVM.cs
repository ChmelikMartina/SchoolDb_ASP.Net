using Asp.NETSchool.Models;
using Microsoft.AspNetCore.Identity;

namespace Asp.NETSchool.ViewModels {
    public class RoleEditVM {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }
}
