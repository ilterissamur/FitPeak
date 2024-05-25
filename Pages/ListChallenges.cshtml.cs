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

        public async Task<IActionResult> OnGetAsync()
        {
            Challenges = await _context.Challenges.ToListAsync();
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

            _context.ApplicationUserChallenges.Add(new ApplicationUserChallenge { ApplicationUser = user, Challenge = challenge });
            await _context.SaveChangesAsync();

            return RedirectToPage("/ListChallenges");
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
