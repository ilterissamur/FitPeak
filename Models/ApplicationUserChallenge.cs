namespace FitPeak.Models
{
    public partial class ApplicationUserChallenge
    {
        public string ApplicationUserId { get; set; }
        public int ChallengeId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Challenge Challenge { get; set; }
    }
}