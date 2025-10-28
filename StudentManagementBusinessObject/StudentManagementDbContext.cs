using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StudentManagementBusinessObject
{
    public class StudentManagementDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassSubject> ClassSubjects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<ClassTask> ClassTasks { get; set; }
        public DbSet<StudentTask> StudentTasks { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        public StudentManagementDbContext() { }

        public StudentManagementDbContext(DbContextOptions<StudentManagementDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                string connectionString = config.GetConnectionString("DB");
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    // Fallback to hardcoded connection string if appsettings.json not found
                    connectionString = "Server=(local);Database=OnlineEduTask;Uid=sa;Pwd=123;TrustServerCertificate=True";
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure primary keys
            modelBuilder.Entity<Student>()
                .HasKey(s => s.StudentId);

            modelBuilder.Entity<StudentClass>()
                .HasKey(sc => sc.ClassId);

            modelBuilder.Entity<Subject>()
                .HasKey(sub => sub.SubjectId);

            modelBuilder.Entity<Task>()
                .HasKey(t => t.TaskId);

            modelBuilder.Entity<ClassTask>()
                .HasKey(ct => ct.ClassTaskId);

            modelBuilder.Entity<StudentTask>()
                .HasKey(st => st.StudentTaskId);

            modelBuilder.Entity<UserAccount>()
                .HasKey(u => u.UserAccountId);

            // Configure ClassSubject (many-to-many between Class and Subject)
            modelBuilder.Entity<ClassSubject>()
                .HasKey(cs => new { cs.ClassId, cs.SubjectId });

            modelBuilder.Entity<ClassSubject>()
                .HasOne(cs => cs.StudentClass)
                .WithMany(c => c.ClassSubjects)
                .HasForeignKey(cs => cs.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSubject>()
                .HasOne(cs => cs.Subject)
                .WithMany(s => s.ClassSubjects)
                .HasForeignKey(cs => cs.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSubject>()
                .HasOne(cs => cs.Teacher)
                .WithMany()
                .HasForeignKey(cs => cs.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Student-StudentClass relationship
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Task -> CreatedByTeacher relationship
            modelBuilder.Entity<Task>()
                .HasOne(t => t.CreatedByTeacher)
                .WithMany()
                .HasForeignKey(t => t.CreatedByTeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ClassTask relationships
            modelBuilder.Entity<ClassTask>()
                .HasOne(ct => ct.Task)
                .WithMany(t => t.ClassTasks)
                .HasForeignKey(ct => ct.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassTask>()
                .HasOne(ct => ct.StudentClass)
                .WithMany()
                .HasForeignKey(ct => ct.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassTask>()
                .HasOne(ct => ct.Subject)
                .WithMany()
                .HasForeignKey(ct => ct.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure StudentTask relationships
            modelBuilder.Entity<StudentTask>()
                .HasOne(st => st.ClassTask)
                .WithMany(ct => ct.StudentTasks)
                .HasForeignKey(st => st.ClassTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentTask>()
                .HasOne(st => st.Student)
                .WithMany()
                .HasForeignKey(st => st.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add unique constraint to prevent duplicate submissions
            modelBuilder.Entity<StudentTask>()
                .HasIndex(st => new { st.ClassTaskId, st.StudentId })
                .IsUnique();
        }
    }
}
