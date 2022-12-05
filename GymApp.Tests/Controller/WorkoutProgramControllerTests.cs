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
    public class WorkoutProgramControllerTests
    {
        private WorkoutProgramController _workoutProgramController;
        private IWorkoutProgramRepository _workoutProgramRepository;
        private IPhotoService _photoService;
        private IHttpContextAccessor _httpContextAccessor;

        public WorkoutProgramControllerTests()
        {
            //Dependencies
            _workoutProgramRepository = A.Fake<IWorkoutProgramRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<HttpContextAccessor>();

            //SUT
            _workoutProgramController = new WorkoutProgramController(_workoutProgramRepository, _photoService, _httpContextAccessor);
        }

        [Fact]
        public void WorkoutProgramController_Index_ReturnSuccess()
        {
            //Arrange - What do i need to bring in?
            var workoutPrograms = A.Fake<IEnumerable<WorkoutProgram>>();
            A.CallTo(() => _workoutProgramRepository.GetAll()).Returns(workoutPrograms);
            //Act
            var result = _workoutProgramController.Index();
            //Assert - Object check actions
            result.Should().BeOfType<Task<IActionResult>>();
        }
        [Fact]
        public void WorkoutProgramController_Detail_ReturnSuccess()
        {
            //Arrange
            var id = 1;
            var workoutProgram = A.Fake<WorkoutProgram>();
            A.CallTo(() => _workoutProgramRepository.GetByIdAsync(id)).Returns(workoutProgram);

            //Act
            var result = _workoutProgramController.Detail(id);
            //Assert - Object check actions
            result.Should().BeOfType<Task<IActionResult>>();

        }
    }
}
