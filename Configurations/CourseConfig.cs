using Assignment_03_EFC_.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_03_EFC_.Configurations
{
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> b)
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.Title).IsRequired().HasMaxLength(200);
            b.Property(c => c.Description).HasMaxLength(1000);
            b.Property(c => c.Price).HasColumnType("decimal(10,2)").IsRequired();
            b.Property(c => c.Level).HasConversion<string>().HasMaxLength(20).IsRequired();
            b.Property(c => c.IsPublished).IsRequired();
            b.Property(c => c.PublishedDate);

            b.HasOne(c => c.Instructor)
             .WithMany(i => i.Courses)
             .HasForeignKey(c => c.InstructorId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(c => c.Category)
             .WithMany(cat => cat.Courses)
             .HasForeignKey(c => c.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasData(
                new Course { Id = 1, Title = "Basic C#", Price = 49m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2023, 1, 5), InstructorId = 1, CategoryId = 3 },
                new Course { Id = 2, Title = "ASP.NET Core", Price = 99m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedDate = new DateTime(2023, 5, 10), InstructorId = 1, CategoryId = 5 },
                new Course { Id = 3, Title = "Advanced EF Core", Price = 129m, Level = CourseLevel.Advanced, IsPublished = false, PublishedDate = null, InstructorId = 3, CategoryId = 5 },
                new Course { Id = 4, Title = "HTML & CSS", Price = 29m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2022, 11, 1), InstructorId = 2, CategoryId = 4 },
                new Course { Id = 5, Title = "Complete JavaScript", Price = 79m, Level = CourseLevel.Intermediate, IsPublished = false, PublishedDate = null, InstructorId = 2, CategoryId = 4 },
                new Course { Id = 6, Title = "Practical UI/UX", Price = 59m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2023, 2, 14), InstructorId = 3, CategoryId = 2 }
            );
        }
    }
}
