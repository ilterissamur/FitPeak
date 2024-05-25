namespace FitPeak.Models
{
    public partial class Challenge
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Difficulty { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<ApplicationUserChallenge> Users { get; set; }

    }
}