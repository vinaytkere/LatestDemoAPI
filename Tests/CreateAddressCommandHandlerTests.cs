using Application.Features.Address.Commands;
using AutoMapper;
using Domain;
using Moq;
using static Application.Interfaces.IRepository;
using FluentAssertions;
using Bogus;

namespace Tests
{
    public class CreateAddressCommandHandlerTests
    {
        private readonly Mock<IRepository<Address>> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        [Fact]
        public async Task CreateAddress_ShouldReturnSuccess_WhenValid()
        {
            // Arrange
            var faker = new Faker<CreateAddressCommand>()
                .RuleFor(x => x.Country, f => f.Address.Country())
                .RuleFor(x => x.State, f => f.Address.State())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.PinCode, f => f.Address.ZipCode())
                .RuleFor(x => x.LandMark, f => f.Address.SecondaryAddress());

            var command = faker.Generate();

            var address = new Address { Id = Guid.NewGuid() };
            _mapperMock.Setup(m => m.Map<Address>(command)).Returns(address);
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Address>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.FromResult(1));

            var handler = new CreateAddressCommandHandler(_repoMock.Object, _mapperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(address.Id);
        }

        [Fact]
        public async Task CreateAddress_ShouldFailValidation_WhenInvalid()
        {
            // Arrange: bogus data that fails validation (e.g., bad pin code)
            var faker = new Faker<CreateAddressCommand>()
                .RuleFor(x => x.Country, f => "")  // required field empty
                .RuleFor(x => x.State, f => "")
                .RuleFor(x => x.City, f => "")
                .RuleFor(x => x.PinCode, f => "abc")  // invalid format
                .RuleFor(x => x.LandMark, f => f.Lorem.Word());

            var command = faker.Generate();

            var validator = new CreateAddressCommandValidator();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(e => e.PropertyName == "Country");
            result.Errors.Should().Contain(e => e.PropertyName == "PinCode");
        }
    }
}