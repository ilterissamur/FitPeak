using Microsoft.AspNetCore.Identity;

namespace FitPeak.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        public byte[] ProfilePicture { get; set; }
        public string Biography { get; set; }
        public ICollection<ApplicationUserChallenge> Challenges { get; set; }
    }
}