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
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> b)
        {
            b.HasKey(s => s.Id);
            b.Property(s => s.FullName).IsRequired().HasMaxLength(100);
            b.Property(s => s.Email).IsRequired().HasMaxLength(150);
            b.HasIndex(s => s.Email).IsUnique();
            b.Property(s => s.DateOfBirth).IsRequired();
        }
    }
}
