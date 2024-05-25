using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitPeak.Models;
using FitPeak.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
namespace FitPeak.Pages

{
    [Authorize]
    public class CreateChallengeModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public Challenge NewChallenge { get; set; }

        public CreateChallengeModel(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public void OnGet()
        {
            NewChallenge = new Challenge();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbContext.Challenges.Add(NewChallenge);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("./ListChallenges");
        }
    }
}
