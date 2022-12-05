using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using GymApp.Data;
using GymApp.Data.Enum;
using GymApp.Models;
using GymApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GymApp.Tests.Repository
{
    public class WorkoutProgramRepositoryTests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Gyms.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.WorkoutPrograms.Add(
                      new WorkoutProgram()
                      {
                          Title = "3DaysPerWeek",
                          Description = "3 Days full body",
                          Image = "https://www.shutterstock.com/image-photo/modern-light-gym-sports-equipment-barbells-721723381",
                          WorkoutProgramCategory = WorkoutProgramCategory.ThreeDayPerWeekBeginner,
                      });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
        [Fact]
        public async void WorkoutProgramRepository_Add_ReturnsBool()
        {
            //Arrange
            var workout = new WorkoutProgram()
            {
                Title = "3DaysPerWeek",
                Description = "3 Days full body",
                Image = "https://www.shutterstock.com/image-photo/modern-light-gym-sports-equipment-barbells-721723381",
                WorkoutProgramCategory = WorkoutProgramCategory.ThreeDayPerWeekBeginner,
            };

            var dbContext = await GetDbContext();
            var workoutRepository = new WorkoutProgramRepository(dbContext);
            //Act
            var result = workoutRepository.Add(workout);

            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async void WorkoutProgramRepository_GetByIdAsync_ReturnsWorkoutProgram()
        {
            //Arrange
            var id = 1;
            var dbContext = await GetDbContext();
            var workoutRepository = new WorkoutProgramRepository(dbContext);

            //Act
            var result = workoutRepository.GetByIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<WorkoutProgram>>();
        }
        [Fact]
        public async void WorkoutProgramRepository_GetAll_ReturnsList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var workoutRepository = new WorkoutProgramRepository(dbContext);


            //Act
            var result = await workoutRepository.GetAll();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<WorkoutProgram>>();
        }        

        [Fact]
        public async void WorkoutProgramRepository_SuccessfulDelete_ReturnsTrue()
        {
            //Arrange
            var workout = new WorkoutProgram()
            {
                Title = "3DaysPerWeek",
                Description = "3 Days full body",
                Image = "https://www.shutterstock.com/image-photo/modern-light-gym-sports-equipment-barbells-721723381",
                WorkoutProgramCategory = WorkoutProgramCategory.ThreeDayPerWeekBeginner,
            };

            var dbContext = await GetDbContext();
            var workoutRepository = new WorkoutProgramRepository(dbContext);

            //Act
            workoutRepository.Add(workout);
            var result = workoutRepository.Delete(workout);

            //Assert
            result.Should().BeTrue();
            
        }
       
    }
}
