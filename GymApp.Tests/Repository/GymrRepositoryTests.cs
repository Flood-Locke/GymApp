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
    public class GymrRepositoryTests
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
                    databaseContext.Gyms.Add(
                      new Gym()
                      {
                          Title = "Gym 1",
                          Image = "https://www.shutterstock.com/shutterstock/photos/721723381/display_1500/stock-photo-modern-light-gym-sports-equipment-in-gym-barbells-of-different-weight-on-rack-721723381.jpg",
                          Description = "This is the description of the first Gym",
                          GymCategory = GymCategory.WeightLifting,
                          Address = new Address()
                          {
                              Street = "123 Main St",
                              City = "Dublin",
                              Province = "Leinster"
                          }
                      });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
        [Fact]
        public async void GymRepository_Add_ReturnsBool()
        {
            //Arrange
            var gym = new Gym()
            {
                Title = "Gym 1",
                Image = "https://www.shutterstock.com/shutterstock/photos/721723381/display_1500/stock-photo-modern-light-gym-sports-equipment-in-gym-barbells-of-different-weight-on-rack-721723381.jpg",
                Description = "This is the description of the first Gym",
                GymCategory = GymCategory.WeightLifting,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Dublin",
                    Province = "Leinster"
                }
            };
            var dbContext = await GetDbContext();
            var gymRepository = new GymRepository(dbContext);
            //Act
            var result = gymRepository.Add(gym);

            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async void GymRepository_GetByIdAsync_ReturnsGym()
        {
            //Arrange
            var id = 1;
            var dbContext = await GetDbContext();
            var gymRepository = new GymRepository(dbContext);

            //Act
            var result = gymRepository.GetByIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Gym>>();
        }
        [Fact]
        public async void GymRepository_GetAll_ReturnsList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var gymRepository = new GymRepository(dbContext);

            //Act
            var result = await gymRepository.GetAll();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Gym>>();
        }
        [Fact]
        public async void GymRepository_GetGymsByCity_ReturnsEmpty()
        {
            //Arrange
            var city = "Limerick";
            var gym = new Gym()
            {
                Title = "Gym 1",
                Image = "https://www.shutterstock.com/shutterstock/photos/721723381/display_1500/stock-photo-modern-light-gym-sports-equipment-in-gym-barbells-of-different-weight-on-rack-721723381.jpg",
                Description = "This is the description of the first gym",
                GymCategory = GymCategory.PowerLifting,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Dublin",
                    Province = "Leinster"
                }
            };
            var dbContext = await GetDbContext();
            var gymRepository = new GymRepository(dbContext);

            //Act
            gymRepository.Add(gym);
            var result = await gymRepository.GetGymByCity(city);

            //Assert - Should be empty as the city passed is not the same as the city on record
            result.Should().BeEmpty();
            //result.Should().BeOfType<List<Gym>>();
            //result.First().Title.Should().Be("Gym 1");
        }

        [Fact]
        public async void GymRepository_SuccessfulDelete_ReturnsTrue()
        {
            //Arrange
            var gym = new Gym()
            {
                Title = "Gym 1",
                Image = "https://www.shutterstock.com/image-photo/modern-light-gym-sports-equipment-barbells-721723381",
                Description = "This is the description of the first gym",
                GymCategory = GymCategory.PowerLifting,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Meath",
                    Province = "Leinster"
                }
            };
            var dbContext = await GetDbContext();
            var gymRepository = new GymRepository(dbContext);

            //Act
            gymRepository.Add(gym);
            var result = gymRepository.Delete(gym);
            var count = await gymRepository.GetCountAsync();

            //Assert
            result.Should().BeTrue();
            count.Should().Be(0);
        }

        [Fact]
        public async void GymRepository_GetCountAsync_ReturnsInt()
        {
            //Arrange
            var gym = new Gym()
            {
                Title = "Gym 1",
                Image = "https://www.shutterstock.com/image-photo/modern-light-gym-sports-equipment-barbells-721723381",
                Description = "This is the description of the first gym",
                GymCategory = GymCategory.PowerLifting,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Meath",
                    Province = "Leinster"
                }
            };
            var dbContext = await GetDbContext();
            var gymRepository = new GymRepository(dbContext);

            //Act
            gymRepository.Add(gym);
            var result = await gymRepository.GetCountAsync();

            //Assert
            result.Should().Be(1);

        }
        [Fact]
        public async void GymRepository_GetAllProvinces_ReturnsList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var gymRepository = new GymRepository(dbContext);

            //Act
            var result = await gymRepository.GetAllProvinces();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Province>>();
        }
        [Fact]
        public async void GymRepository_GetGymsByProvince_ReturnsList()
        {
            //Arrange
            var province = "Leinster";
            var gym = new Gym()
            {
                Title = "Gym 1",
                Image = "https://www.shutterstock.com/image-photo/modern-light-gym-sports-equipment-barbells-721723381",
                Description = "This is the description of the first gym",
                GymCategory = GymCategory.PowerLifting,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Meath",
                    Province = "Leinster"
                }
            };
            var dbContext = await GetDbContext();
            var gymRepository = new GymRepository(dbContext);

            //Act
            gymRepository.Add(gym);
            var result = await gymRepository.GetGymsByProvince(province);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Gym>>();
            result.First().Title.Should().Be("Gym 1");
        }
        [Fact]
        public async void GymRepository_GetGymsByProvince_ReurnsEmpty()
        {
            //Arrange
            var province = "Munster";
            var gym = new Gym()
            {
                Title = "Gym 1",
                Image = "https://www.shutterstock.com/image-photo/modern-light-gym-sports-equipment-barbells-721723381",
                Description = "This is the description of the first gym",
                GymCategory = GymCategory.PowerLifting,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Meath",
                    Province = "Leinster"
                }
            };
            var dbContext = await GetDbContext();
            var gymRepository = new GymRepository(dbContext);

            //Act
            gymRepository.Add(gym);
            var result = await gymRepository.GetGymsByProvince(province);

            //Assert
            result.Should().BeEmpty();
            
        }
    }
}
