using Microsoft.AspNetCore.Identity;

namespace Marketplace
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}