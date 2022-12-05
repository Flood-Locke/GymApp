using GymApp.Data.Enum;
using GymApp.Models;
using Microsoft.AspNetCore.Identity;

namespace GymApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Gyms.Any())
                {
                    context.Gyms.AddRange(new List<Gym>()
                    {
                        new Gym()
                        {
                            Title = "Gym 1",
                            Image = "https://www.shutterstock.com/image-photo/modern-light-gym-sports-equipment-600w-721723381.jpg",
                            Description = "This is the description of the first Gym",
                            GymCategory = GymCategory.PowerLifting,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Dublin",
                                Province = "Leinster"
                            }
                         },
                        new Gym()
                        {
                            Title = "Gym 2",
                            Image = "https://www.shutterstock.com/image-photo/modern-light-gym-sports-equipment-600w-721723381.jpg",
                            Description = "This is the description of the second Gym",
                            GymCategory = GymCategory.CrossFit,
                            Address = new Address()
                            {
                                Street = "13 Less St",
                                City = "Cork",
                                Province = "Munster"
                            }
                        },
                        new Gym()
                        {
                            Title = "Gym 3",
                            Image = "https://www.shutterstock.com/image-photo/modern-light-gym-sports-equipment-600w-721723381.jpg",
                            Description = "This is the description of the third Gym",
                            GymCategory = GymCategory.WeightLifting,
                            Address = new Address()
                            {
                                Street = "123 Up St",
                                City = "Belfast",
                                Province = "Ulster"
                            }
                        },
                        new Gym()
                        {
                            Title = "Gym 4",
                            Image = "https://www.shutterstock.com/image-photo/modern-light-gym-sports-equipment-600w-721723381.jpg",
                            Description = "This is the description of the fourth Gym",
                            GymCategory = GymCategory.WeightLifting,
                            Address = new Address()
                            {
                                Street = "123 Down St",
                                City = "Galway",
                                Province = "Connacht"
                            }
                        }
                    });
                    context.SaveChanges();
                }
                //Workoutprograms
                if (!context.WorkoutPrograms.Any())
                {
                    context.WorkoutPrograms.AddRange(new List<WorkoutProgram>()
                    {
                        new WorkoutProgram()
                        {
                            Title = "Workout Program 1",
                            Image = "https://www.shutterstock.com/shutterstock/photos/461236648/display_1500/stock-photo-closeup-of-weightlifter-clapping-hands-before-barbell-workout-at-the-gym-461236648.jpg",
                            Description = "This is the description of the first program",
                            WorkoutProgramCategory = WorkoutProgramCategory.ThreeDayPerWeekBeginner
                           
                        },
                        new WorkoutProgram()
                        {
                            Title = "Workout Program 2",
                            Image = "https://www.shutterstock.com/shutterstock/photos/461236648/display_1500/stock-photo-closeup-of-weightlifter-clapping-hands-before-barbell-workout-at-the-gym-461236648.jpg",
                            Description = "This is the description of the second program",
                            WorkoutProgramCategory = WorkoutProgramCategory.FourDayPerWeekBeginner,                           
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "karlmdeveloper@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "karlmdev",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Dublin",
                            Province = "Leinster"
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@fakeemail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Up St",
                            City = "Belfast",
                            Province = "Ulster"
                        }
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
