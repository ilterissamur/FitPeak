using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FitPeak.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FitPeak.Data;
using Microsoft.AspNetCore.Authorization;

namespace FitPeak.Pages
{
    [Authorize]
    public class ListChallengesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<Challenge> Challenges { get; set; }

        public ListChallengesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string sort)
        {
            Challenges = await _context.Challenges.ToListAsync();

            if (sort == "easy")
            {
                Challenges = Challenges.OrderBy(c => c.Difficulty == "Medium" ? 2 : c.Difficulty == "Easy" ? 1 : 3).ToList();
            }
            else if (sort == "hard")
            {
                Challenges = Challenges.OrderBy(c => c.Difficulty == "Hard" ? 1 : c.Difficulty == "Medium" ? 2 : 3).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(string option)
        {
            Challenges = await _context.Challenges.ToListAsync();

            if (option == "hard")
            {
                Challenges = Challenges.Where(c => c.Difficulty == "Hard").ToList();
            }
            else if (option == "medium")
            {
                Challenges = Challenges.Where(c => c.Difficulty == "Medium").ToList();
            }
            else if (option == "easy")
            {
                Challenges = Challenges.Where(c => c.Difficulty == "Easy").ToList();
            }
            else if (option == "step")
            {
                Challenges = Challenges.Where(c => c.Category == "Step").ToList();
            }
            else if (option == "cardio")
            {
                Challenges = Challenges.Where(c => c.Category == "Cardio").ToList();
            }
            else if (option == "squad")
            {
                Challenges = Challenges.Where(c => c.Category == "Squad").ToList();
            }
            else if (option == "wloss")
            {
                Challenges = Challenges.Where(c => c.Category == "Weight Loss").ToList();
            }

            return Page();
        }



        public async Task<IActionResult> OnPostJoinAsync(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = "/ListChallenges" });
            }

            _context.ApplicationUserChallenges.Add(new ApplicationUserChallenge { ApplicationUser = user, Challenge = challenge, });
            await _context.SaveChangesAsync();

            return RedirectToPage("/ListChallenges");
        }

        public async Task<IActionResult> OnPostFavAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var favRow = await _context.ApplicationUserChallenges
           .FirstOrDefaultAsync(row => row.ApplicationUserId == user.Id && row.ChallengeId == id);

            favRow.IsFavorite = true;

            await _context.SaveChangesAsync();
            return RedirectToPage("/ListChallenges");
        }

        public async Task<bool> UserHasFavoritedChallenge(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var favRow = await _context.ApplicationUserChallenges
           .FirstOrDefaultAsync(row => row.ApplicationUserId == user.Id && row.ChallengeId == id);

            if (favRow == null)
            {
                return false;
            }
            else if (favRow.IsFavorite)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UserHasJoinedChallenge(int challengeId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return false;
            }

            return await _context.ApplicationUserChallenges
                .AnyAsync(ac => ac.ApplicationUserId == user.Id && ac.ChallengeId == challengeId);
        }
    }
}
