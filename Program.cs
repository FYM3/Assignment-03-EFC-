using Assignment_03_EFC_.AppDbContexts;
using Assignment_03_EFC_.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment_03_EFC_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new AppDbContext();

            #region Students & Enrollments

            SeedStudentsAndEnrollments();

            #endregion

            #region Create

            AddInstructor();
            AddCourse();
            AddSeveralStudents();
            EnrollStudent();

            #endregion

            #region Read

            FindCourse();
            FilterCourses(30m, 120m);
            PrintSubCategories(1);
            PrintCourseRoster(1);

            #endregion

            #region Update

            ChangePrice(1, 55m);
            UpdateProgress(1, 1, 100);
            PublishCourse(3);

            #endregion

            #region Delete

            Unenroll(1, 2);
            TryDeleteCategoryWithChildren(1);
            SafeDeleteCategory(2);
            DeleteCourseWithEnrollments(1);

            #endregion

            #region Bulk Operations

            BulkPriceRaise(CourseLevel.Intermediate, 0.10m);
            BulkDeleteDroppedEnrollments();

            #endregion

            // ============================================================================
            // Operation Methods
            // ============================================================================

            #region Seed

            void SeedStudentsAndEnrollments()
            {
                if (db.Students.Any())
                {
                    Console.WriteLine(">> Students already exist. Skipping seed.");
                    return;
                }

                var students = new List<Student>
    {
        new() { FullName = "Khaled Mohamed", Email = "khaled@mail.com", DateOfBirth = new DateTime(2000, 5, 12) },
        new() { FullName = "Layla Ahmed",    Email = "layla@mail.com",  DateOfBirth = new DateTime(1999, 8, 20) },
        new() { FullName = "Omar Saeed",     Email = "omar@mail.com",   DateOfBirth = new DateTime(2001, 2, 3)  },
        new() { FullName = "Huda Karim",     Email = "huda@mail.com",   DateOfBirth = new DateTime(1998, 11, 30)},
        new() { FullName = "Yousef Ali",     Email = "yousef@mail.com", DateOfBirth = new DateTime(2000, 9, 17) }
    };
                db.Students.AddRange(students);
                db.SaveChanges();

                var enrollments = new List<Enrollment>
    {
        new() { StudentId = students[0].Id, CourseId = 1, EnrollmentDate = DateTime.Now.AddDays(-20), ProgressPercentage = 100, Status = EnrollmentStatus.Completed, FinalGrade = 95m },
        new() { StudentId = students[0].Id, CourseId = 2, EnrollmentDate = DateTime.Now.AddDays(-10), ProgressPercentage = 40,  Status = EnrollmentStatus.Active },
        new() { StudentId = students[1].Id, CourseId = 1, EnrollmentDate = DateTime.Now.AddDays(-15), ProgressPercentage = 100, Status = EnrollmentStatus.Completed, FinalGrade = 88m },
        new() { StudentId = students[1].Id, CourseId = 4, EnrollmentDate = DateTime.Now.AddDays(-5),  ProgressPercentage = 20,  Status = EnrollmentStatus.Active },
        new() { StudentId = students[2].Id, CourseId = 2, EnrollmentDate = DateTime.Now.AddDays(-30), ProgressPercentage = 0,   Status = EnrollmentStatus.Dropped },
        new() { StudentId = students[3].Id, CourseId = 6, EnrollmentDate = DateTime.Now.AddDays(-7),  ProgressPercentage = 60,  Status = EnrollmentStatus.Active },
        new() { StudentId = students[4].Id, CourseId = 1, EnrollmentDate = DateTime.Now.AddDays(-40), ProgressPercentage = 100, Status = EnrollmentStatus.Completed, FinalGrade = 75m },
        new() { StudentId = students[4].Id, CourseId = 4, EnrollmentDate = DateTime.Now.AddDays(-3),  ProgressPercentage = 0,   Status = EnrollmentStatus.Dropped }
    };
                db.Enrollments.AddRange(enrollments);
                db.SaveChanges();

                Console.WriteLine($">> Seeded {students.Count} students and {enrollments.Count} enrollments.");
            }

            #endregion

            #region Create Operations

            void AddInstructor()
            {
                var instructor = new Instructor
                {
                    FullName = "Noura Al-Otaibi",
                    Email = $"noura{DateTime.Now.Ticks}@edu.com",
                    Bio = "New instructor.",
                    JoinedOn = DateTime.Now
                };
                db.Instructors.Add(instructor);
                db.SaveChanges();
                Console.WriteLine($">> Added instructor with Id: {instructor.Id}");
            }

            void AddCourse()
            {
                int instructorId = 1, categoryId = 5;

                if (!db.Instructors.Any(i => i.Id == instructorId))
                {
                    Console.WriteLine(">> Instructor not found.");
                    return;
                }

                if (!db.Categories.Any(c => c.Id == categoryId))
                {
                    Console.WriteLine(">> Category not found.");
                    return;
                }

                var course = new Course
                {
                    Title = "Blazor From Scratch",
                    Description = "A new course.",
                    Price = 89m,
                    Level = CourseLevel.Intermediate,
                    IsPublished = false,
                    InstructorId = instructorId,
                    CategoryId = categoryId
                };
                db.Courses.Add(course);
                db.SaveChanges();
                Console.WriteLine($">> Added course with Id: {course.Id}");
            }

            void AddSeveralStudents()
            {
                var students = new List<Student>
    {
        new() { FullName = "Student One",   Email = $"s1_{DateTime.Now.Ticks}@mail.com", DateOfBirth = new DateTime(2000, 1, 1) },
        new() { FullName = "Student Two",   Email = $"s2_{DateTime.Now.Ticks}@mail.com", DateOfBirth = new DateTime(2001, 2, 2) },
        new() { FullName = "Student Three", Email = $"s3_{DateTime.Now.Ticks}@mail.com", DateOfBirth = new DateTime(2002, 3, 3) }
    };
                db.Students.AddRange(students);
                db.SaveChanges();
                Console.WriteLine($">> Added students with Ids: {string.Join(", ", students.Select(s => s.Id))}");
            }

            void EnrollStudent()
            {
                int studentId = 2, courseId = 6;

                var student = db.Students.Find(studentId);
                if (student is null) { Console.WriteLine(">> Student not found."); return; }

                var course = db.Courses.Find(courseId);
                if (course is null) { Console.WriteLine(">> Course not found."); return; }

                if (!course.IsPublished)
                {
                    Console.WriteLine(">> Course is not published. Enrollment refused.");
                    return;
                }

                if (db.Enrollments.Any(e => e.StudentId == studentId && e.CourseId == courseId))
                {
                    Console.WriteLine(">> Student is already enrolled in this course.");
                    return;
                }

                db.Enrollments.Add(new Enrollment
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    EnrollmentDate = DateTime.Now,
                    ProgressPercentage = 0,
                    Status = EnrollmentStatus.Active
                });
                db.SaveChanges();
                Console.WriteLine(">> Enrollment successful.");
            }

            #endregion

            #region Read Operations

            void FindCourse()
            {
                var byId = db.Courses.Find(1);
                Console.WriteLine(byId is null
                    ? ">> Not found (Find)"
                    : $">> Find: {byId.Title}");

                var byTitle = db.Courses.FirstOrDefault(c => c.Title == "ASP.NET Core");
                Console.WriteLine(byTitle is null
                    ? ">> Not found (FirstOrDefault)"
                    : $">> FirstOrDefault: {byTitle.Title}");
            }

            void FilterCourses(decimal min, decimal max)
            {
                var list = db.Courses
                    .Where(c => c.Price >= min && c.Price <= max)
                    .OrderByDescending(c => c.Price)
                    .Select(c => new { c.Title, c.Level, c.Price })
                    .ToList();

                Console.WriteLine($">> Courses priced between {min} and {max}:");
                foreach (var c in list)
                    Console.WriteLine($"   - {c.Title} | {c.Level} | {c.Price}");
            }

            void PrintSubCategories(int parentId)
            {
                var parent = db.Categories.Find(parentId);
                if (parent is null) { Console.WriteLine(">> Category not found."); return; }

                var subs = db.Categories.Where(c => c.ParentCategoryId == parentId).ToList();
                Console.WriteLine($">> Direct sub-categories of '{parent.Name}':");
                if (subs.Count == 0) Console.WriteLine("   (none)");
                foreach (var s in subs) Console.WriteLine($"   - {s.Name}");

                var top = db.Categories.Where(c => c.ParentCategoryId == null).ToList();
                Console.WriteLine(">> Top-level categories:");
                foreach (var t in top) Console.WriteLine($"   - {t.Name}");
            }

            void PrintCourseRoster(int courseId)
            {
                var course = db.Courses.Find(courseId);
                if (course is null) { Console.WriteLine(">> Course not found."); return; }

                var studentIds = db.Enrollments
                    .Where(e => e.CourseId == courseId)
                    .Select(e => e.StudentId)
                    .ToList();

                var students = db.Students
                    .Where(s => studentIds.Contains(s.Id))
                    .ToList();

                Console.WriteLine($">> Enrolled students in '{course.Title}': {students.Count}");
                foreach (var s in students) Console.WriteLine($"   - {s.FullName}");
            }

            #endregion

            #region Update Operations

            void ChangePrice(int courseId, decimal newPrice)
            {
                var course = db.Courses.Find(courseId);
                if (course is null) { Console.WriteLine(">> Course not found."); return; }

                course.Price = newPrice;
                db.SaveChanges();
                Console.WriteLine($">> Price of '{course.Title}' changed to {newPrice}.");
            }

            void UpdateProgress(int studentId, int courseId, int progress)
            {
                if (progress < 0 || progress > 100)
                {
                    Console.WriteLine(">> Progress must be between 0 and 100.");
                    return;
                }

                var enrollment = db.Enrollments.Find(studentId, courseId);
                if (enrollment is null) { Console.WriteLine(">> Enrollment not found."); return; }

                enrollment.ProgressPercentage = progress;
                if (progress == 100) enrollment.Status = EnrollmentStatus.Completed;

                db.SaveChanges();
                Console.WriteLine($">> Progress updated to {progress}% (Status: {enrollment.Status}).");
            }

            void PublishCourse(int courseId)
            {
                var course = db.Courses.Find(courseId);
                if (course is null) { Console.WriteLine(">> Course not found."); return; }

                course.IsPublished = true;
                course.PublishedDate = DateTime.Now;
                db.SaveChanges();
                Console.WriteLine($">> '{course.Title}' published on {course.PublishedDate:yyyy-MM-dd}.");
            }

            #endregion

            #region Delete Operations

            void Unenroll(int studentId, int courseId)
            {
                var enrollment = db.Enrollments.Find(studentId, courseId);
                if (enrollment is null) { Console.WriteLine(">> Enrollment not found."); return; }

                db.Enrollments.Remove(enrollment);
                db.SaveChanges();
                Console.WriteLine(">> Enrollment removed.");
            }

            void TryDeleteCategoryWithChildren(int categoryId)
            {
                var category = db.Categories.Find(categoryId);
                if (category is null) { Console.WriteLine(">> Category not found."); return; }

                try
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();
                    Console.WriteLine(">> Deleted (unexpected!).");
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($">> Delete failed: {ex.InnerException?.Message ?? ex.Message}");
                }
                finally
                {
                    // Reset state to avoid tracking issues in the same context.
                    db.Entry(category).State = EntityState.Unchanged;
                }
            }

            void SafeDeleteCategory(int categoryId)
            {
                var category = db.Categories.Find(categoryId);
                if (category is null) { Console.WriteLine(">> Category not found."); return; }

                if (db.Categories.Any(c => c.ParentCategoryId == categoryId))
                {
                    Console.WriteLine(">> Cannot delete: category has sub-categories.");
                    return;
                }

                if (db.Courses.Any(c => c.CategoryId == categoryId))
                {
                    Console.WriteLine(">> Cannot delete: category has related courses.");
                    return;
                }

                db.Categories.Remove(category);
                db.SaveChanges();
                Console.WriteLine(">> Category deleted safely.");
            }

            void DeleteCourseWithEnrollments(int courseId)
            {
                var course = db.Courses.Find(courseId);
                if (course is null) { Console.WriteLine(">> Course not found."); return; }

                int before = db.Enrollments.Count(e => e.CourseId == courseId);
                Console.WriteLine($">> Enrollments before delete: {before}");

                // Enrollments are removed automatically because the Enrollment -> Course
                // relationship is configured with DeleteBehavior.Cascade, so SQL Server
                // applies ON DELETE CASCADE when the course is deleted.
                db.Courses.Remove(course);
                db.SaveChanges();

                int after = db.Enrollments.Count(e => e.CourseId == courseId);
                Console.WriteLine($">> Enrollments after delete: {after}");
            }

            #endregion

            #region Bulk Operations

            void BulkPriceRaise(CourseLevel level, decimal rate)
            {
                int affected = db.Courses
                    .Where(c => c.IsPublished && c.Level == level)
                    .ExecuteUpdate(s => s.SetProperty(c => c.Price, c => c.Price * (1 + rate)));

                Console.WriteLine($">> Raised price of {affected} published '{level}' course(s) by {rate:P0}.");
            }

            void BulkDeleteDroppedEnrollments()
            {
                int affected = db.Enrollments
                    .Where(e => e.Status == EnrollmentStatus.Dropped)
                    .ExecuteDelete();

                Console.WriteLine($">> Deleted {affected} 'Dropped' enrollment(s).");
            }

            #endregion

        }
    }
}
