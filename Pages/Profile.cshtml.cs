using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitPeak.Models;
using FitPeak.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
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

                Console.WriteLine($"{temp.Id}");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var biography = user.Biography;
            var picture = user.ProfilePicture;
            Picture = user.ProfilePicture ?? Array.Empty<byte>();
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
