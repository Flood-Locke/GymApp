using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymApp.Controllers;
using GymApp.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Xunit;
using GymApp.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Tests.Controller
{
    //You want to return fake values so that the environment is the same
    //Can't unit test static functions
    public class GymControllerTests
    {
        private GymController _gymController;
        private IGymRepository _gymRepository;
        private IPhotoService _photoService;
        private IHttpContextAccessor _httpContextAccessor;

        public GymControllerTests()
        {
            //Dependencies
            _gymRepository = A.Fake<IGymRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<HttpContextAccessor>();

            //SUT
            _gymController = new GymController(_gymRepository, _photoService);
        }

        [Fact]
        public void GymController_Index_ReturnSuccess()
        {
            //Arrange - What do i need to bring in?
            var gyms = A.Fake<IEnumerable<Gym>>();
            A.CallTo(() => _gymRepository.GetAll()).Returns(gyms);
            //Act
            var result = _gymController.Index();
            //Assert - Object check actions
            result.Should().BeOfType<Task<IActionResult>>();
        }
        [Fact]
        public void GymController_Detail_ReturnSuccess()
        {
            //Arrange
            var id = 1;
            var gym = A.Fake<Gym>();
            A.CallTo(() => _gymRepository.GetByIdAsync(id)).Returns(gym);

            //Act
            var result = _gymController.Detail(id);
            //Assert - Object check actions
            result.Should().BeOfType<Task<IActionResult>>();

        }
    }
}
    