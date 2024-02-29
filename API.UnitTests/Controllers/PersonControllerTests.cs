using API.Controllers;
using Contracts.DTOs.Person;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Services.Interfaces;

namespace API.UnitTests.Controllers
{
    public class PersonControllerTests
    {
        private Mock<IPersonServices> _mockPersonServices;
        private Mock<ILogger<PersonController>> _mockLogger;
         
        public PersonControllerTests()
        {
            _mockPersonServices = new Mock<IPersonServices>();
            _mockLogger = new Mock<ILogger<PersonController>>();
        }

        private PersonController MakePersonControllerInstance()
            => new PersonController(_mockLogger.Object, _mockPersonServices.Object);

        private static FetchPersonDto _fetchPersonDtoSample => new()
        {
            Id = 1,
            FirstName = "mahsa",
            LastName = "abootalebi",
            Email = "m@g.c",
            RawPhoneNumber = 91000000000,
            DateOfBirth = new DateOnly(2024, 2, 29),
        };


        [Fact]
        public async Task GetById_SendValidId_ReturnsOkStatus()
        {
            //arrange
            _mockPersonServices.Setup(t => t.GetPersonByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fetchPersonDtoSample)
                .Verifiable();
            var personController = MakePersonControllerInstance();

            //act
            var act = await personController.GetById(_fetchPersonDtoSample.Id);

            //assert
            var result = act as OkObjectResult;
            Assert.NotNull(result?.Value);
            Assert.IsType<FetchPersonDto>(result?.Value);
            _mockPersonServices.Verify();
        }

        [Fact]
        public async Task GetById_SendInvalidId_ReturnsNotFoundStatus()
        {
            //arrange
            _mockPersonServices.Setup(t => t.GetPersonByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((FetchPersonDto?)null)
                .Verifiable();
            var personController = MakePersonControllerInstance();

            //act
            var act = await personController.GetById(_fetchPersonDtoSample.Id);

            //assert
            var result = act as NotFoundObjectResult;
            Assert.NotNull(result?.Value);
            Assert.IsType<string>(result?.Value);
            _mockPersonServices.Verify();
        }
    }
}