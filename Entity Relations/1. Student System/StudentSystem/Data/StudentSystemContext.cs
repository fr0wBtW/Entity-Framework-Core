using Microsoft.EntityFrameworkCore;
using StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }
        public StudentSystemContext(DbContextOptions options) : base(options)
        {

        }
        DbSet<Course> Courses { get; set; }
        DbSet<Homework> Homeworks { get; set; }
        DbSet<Resource> Resources { get; set; }
        DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-VLH0QE3\\SQLEXPRESS03;Database=StudentSystem;Integrated Security = True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(c => c.Name).IsUnicode(true);

                entity.Property(c => c.Description).IsUnicode(true);
            });
            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasKey(e => e.HomeworkId);

                entity.Property(e => e.SubmissionTime).ValueGeneratedOnAdd();
                
                entity.HasOne(h => h.Student).WithMany(s => s.Homeworks).HasForeignKey(h => h.StudentId);

                entity.HasOne(h => h.Course).WithMany(c => c.Homeworks).HasForeignKey(h => h.CourseId);
            });
            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(r => r.ResourceId);

                entity.Property(r => r.Name).HasMaxLength(50).IsUnicode(true).IsRequired(true);

                entity.Property(r => r.Url).IsUnicode(false).IsRequired(true);

                entity.HasOne(r => r.Course).WithMany(c => c.Resources).HasForeignKey(r => r.CourseId);
            });
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.StudentId);

                entity.Property(s => s.Name).HasMaxLength(100).IsUnicode(true);

                entity.Property(s => s.PhoneNumber).HasMaxLength(10).IsUnicode(false).IsRequired(false);

                entity.Property(s => s.RegisteredOn).ValueGeneratedOnAdd();

                entity.Property(s => s.Birthday).IsRequired(false);
            });
            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new { sc.StudentId, sc.CourseId });

                entity.HasOne(sc => sc.Student).WithMany(s => s.Courses);

                entity.HasOne(sc => sc.Course).WithMany(c => c.Students);
            });

        }
    }
}
