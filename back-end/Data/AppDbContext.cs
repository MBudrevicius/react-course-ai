using Microsoft.EntityFrameworkCore;
using Models.Db;

namespace Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Problem> Problems { get; set; }
    public DbSet<CodeSubmission> Submissions { get; set; }
    public DbSet<ChatHistory> ChatHistory { get; set; }
}
