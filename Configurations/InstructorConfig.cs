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
    public class InstructorConfig : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> b)
        {
            b.HasKey(i => i.Id);
            b.Property(i => i.FullName).IsRequired().HasMaxLength(100);
            b.Property(i => i.Email).IsRequired().HasMaxLength(150);
            b.HasIndex(i => i.Email).IsUnique();
            b.Property(i => i.Bio).HasMaxLength(500);
            b.Property(i => i.JoinedOn).IsRequired();

            b.HasMany(i => i.Courses)
             .WithOne(c => c.Instructor)
             .HasForeignKey(c => c.InstructorId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasData(
                new Instructor { Id = 1, FullName = "Ahmed Ali", Email = "ahmed@edu.com", Bio = "Backend", JoinedOn = new DateTime(2022, 1, 10) },
                new Instructor { Id = 2, FullName = "Mona Hassan", Email = "mona@edu.com", Bio = "Frontend", JoinedOn = new DateTime(2023, 3, 15) },
                new Instructor { Id = 3, FullName = "Sara Adel", Email = "sara@edu.com", Bio = null, JoinedOn = new DateTime(2021, 7, 1) }
            );
        }
    }
}
