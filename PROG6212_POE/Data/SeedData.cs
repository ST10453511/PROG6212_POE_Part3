using PROG6212_POE.Models;

namespace PROG6212_POE.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            // If users already exist, do nothing
            if (context.Users.Any())
            {
                return;
            }

            var users = new User[]
            {
                // 1. HR User (The "Super User" for Part 3)
                new User { Username = "HR", Password = "HR", Role = "HR", FullName = "System Administrator", Email = "hr@cmcs.edu" },

                // 2. Lecturer (With a pre-set Hourly Rate for automation)
                new User { Username = "Lecturer", Password = "Lecturer", Role = "Lecturer", FullName = "John Doe", Email = "john@cmcs.edu", HourlyRate = 500 },

                // 3. Coordinator
                new User { Username = "Coordinator", Password = "Coordinator", Role = "ProgrammeCoordinator", FullName = "Jane Smith", Email = "jane@cmcs.edu" },

                // 4. Manager
                new User { Username = "Manager", Password = "Manager", Role = "AcademicManager", FullName = "Dr. P. Manager", Email = "manager@cmcs.edu" }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}