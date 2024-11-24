using Microsoft.EntityFrameworkCore;
using mtHopeApiProject.Models;

namespace mtHopeApiProject.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<FormSubmission> FormSubmissions { get; set; }
    }
}
