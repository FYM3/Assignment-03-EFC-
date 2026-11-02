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
    public class EnrollmentConfig : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> b)
        {
            b.HasKey(e => new { e.StudentId, e.CourseId });

            b.Property(e => e.EnrollmentDate).IsRequired();
            b.Property(e => e.ProgressPercentage).IsRequired();
            b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            b.Property(e => e.FinalGrade).HasColumnType("decimal(5,2)");

            b.HasOne(e => e.Student)
             .WithMany(s => s.Enrollments)
             .HasForeignKey(e => e.StudentId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(e => e.Course)
             .WithMany(c => c.Enrollments)
             .HasForeignKey(e => e.CourseId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
