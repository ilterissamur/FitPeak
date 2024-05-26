using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitPeak.Models;
using FitPeak.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MyApp.Namespace
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context
        )
        {
            _userManager = userManager;
            _context = context;
        }



        public byte[] Picture { get; set; }
        public string Biography { get; set; }
        public string Username { get; set; }
        public List<Challenge> Challenges = new List<Challenge>();


        private async Task LoadAsync(ApplicationUser user)
        {
            user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == user.Id);

            var favoriteChallengeIds = await _context.ApplicationUserChallenges
            .Where(c => c.ApplicationUserId == user.Id && c.IsFavorite)
            .Select(c => c.ChallengeId)
            .ToListAsync();

            foreach (var challengeId in favoriteChallengeIds)
            {
                var temp = await _context.Challenges.FirstOrDefaultAsync(c => c.Id == challengeId);

                if (temp != null)
                {
                    Challenges.Add(temp);
                }
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var biography = user.Biography;
            var picture = user.ProfilePicture;
            if (user.ProfilePicture != null)
            {
                Picture = user.ProfilePicture ?? Array.Empty<byte>();
            }
            else
            {
                string picturePath = "./wwwroot/images/default_profile_picture.jpeg";
                using var stream = System.IO.File.OpenRead(picturePath);
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                Picture = memoryStream.ToArray();
            }

            Username = userName;
            Biography = biography;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }
    }
}
