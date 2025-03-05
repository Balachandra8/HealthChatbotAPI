using Microsoft.EntityFrameworkCore;

namespace HealthChatbotAPI.Model
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ChatbotSession> ChatbotSessions { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
