namespace QuizBuilder.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> AspNetUsers { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionType> QuestionTypes { get; set; }
    public DbSet<UserQuiz> UserQuizzes { get; set; }
    public DbSet<UserAnsweredQuestion> UserAnsweredQuestion { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserQuiz>().Property(x => x.Score).HasPrecision(5, 2);
        modelBuilder.Entity<UserAnsweredQuestion>().Property(x => x.Score).HasPrecision(5, 2);

        base.OnModelCreating(modelBuilder);
    }
}