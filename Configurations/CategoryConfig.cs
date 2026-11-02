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
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> b)
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.Name).IsRequired().HasMaxLength(100);
            b.Property(c => c.Description).HasMaxLength(500);

            b.HasOne(c => c.ParentCategory)
             .WithMany(c => c.SubCategories)
             .HasForeignKey(c => c.ParentCategoryId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasData(
                new Category { Id = 1, Name = "Programmer", Description = "All programming courses", ParentCategoryId = null },
                new Category { Id = 2, Name = "Designer", Description = "UI/UX design courses", ParentCategoryId = null },
                new Category { Id = 3, Name = "Backend", Description = "Server-side development courses", ParentCategoryId = 1 },
                new Category { Id = 4, Name = "Frontend", Description = "Client-side development courses", ParentCategoryId = 1 },
                new Category { Id = 5, Name = "ASP.NET", Description = ".NET web development courses", ParentCategoryId = 3 }
            );
        }
    }
}
