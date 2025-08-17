using LMS_Quadra.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using System.Data;

namespace LMS_Quadra.Domain
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<WorkerPosition> WorkerPositions { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Worker> Workers { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<CourseCompletion> CourseCompletions { get; set; } = null!;
        public DbSet<CourseContent> CourseContents { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
        public DbSet<IdentityUserRole<string>> UserRoles => Set<IdentityUserRole<string>>();
        public DbSet<IdentityRole> Roles => Set<IdentityRole>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
