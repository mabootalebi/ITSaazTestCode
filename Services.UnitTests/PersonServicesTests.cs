using Domain.Entities;
using Domain.RepositoryInterfaces.IPersonRepository.Commands;
using Domain.RepositoryInterfaces.IPersonRepository.Queries;
using Moq;
using Services.Interfaces;

namespace Services.UnitTests
{
    public class PersonServicesTests
    {
        private Mock<IPersonQueryRepository> _mockQueryRepository;
        private Mock<IPersonCommandRepository> _mockCommandRepository;

        public PersonServicesTests()
        {
            _mockQueryRepository = new Mock<IPersonQueryRepository>();
            _mockCommandRepository = new Mock<IPersonCommandRepository>();
        }

        private IPersonServices MakePersonServiceInstance() 
            => new PersonServices(_mockQueryRepository.Object, _mockCommandRepository.Object);

        private static Person _personSample => new()
        {
            Id = 1,
            Firstname = "mahsa",
            Lastname = "abootalebi",
            Email = "m@g.c",
            PhoneNumber = 91000000000,
            DateOfBirth = new DateOnly(2024,2,29),
        };

        [Fact]
        public async Task GetPersonById_SendValidId_ReturnsPerson()
        {
            //arrange
            var person = _personSample;
            var idSample = person.Id;
            _mockQueryRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(person).Verifiable();

            //act
            var personService = MakePersonServiceInstance();
            var result = await personService.GetPersonByIdAsync(idSample);

            //assert
            Assert.NotNull(result);
            Assert.True(result.PhoneNumber is string);
            Assert.True(result.PhoneNumber.StartsWith('0'));
            _mockQueryRepository.Verify();
        }

        [Fact]
        public async Task GetPersonById_SendInvalidId_ReturnsNull()
        {
            //arrange
            var idSample = _personSample.Id;
            _mockQueryRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Person?)null).Verifiable();

            //act
            var personService = MakePersonServiceInstance();
            var result = await personService.GetPersonByIdAsync(idSample);

            //assert
            Assert.Null(result);
            _mockQueryRepository.Verify();
        }
    }
}