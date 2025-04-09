using Microsoft.EntityFrameworkCore;
using Models.Db;

namespace Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Problem> Problems { get; set; }
    public DbSet<CodeSubmission> Submissions { get; set; }
    public DbSet<ChatHistory> ChatHistory { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
