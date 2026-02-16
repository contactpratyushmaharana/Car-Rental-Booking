using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Application.Usecases;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Interfaces;
using NSubstitute;

namespace CarRentalSystem.Application.Tests.UseCases;

[TestClass]
public sealed class CarRegistrationUseCaseTests
{
    private ICarRegistrationRepository _carRegistrationRepositoryMock;
    private ICarCategoryFactory _carCategoryFactoryMock;
    private ICarCategory _carCategoryMock;
    private RentalPickup _rentalPickupMock;

    private ICarRegistration _systemUnderTest;

    [TestInitialize]
    public void Initialize()
    {
        _carRegistrationRepositoryMock = Substitute.For<ICarRegistrationRepository>();
        _carCategoryFactoryMock = Substitute.For<ICarCategoryFactory>();
        _carCategoryMock = Substitute.For<ICarCategory>();

        _rentalPickupMock = new RentalPickup(
            Guid.NewGuid(),
            "ABC123",
            "123-45-6789",
            _carCategoryMock,
            DateTime.UtcNow,
            1000);

        _systemUnderTest = new CarRegistrationUseCase(_carRegistrationRepositoryMock, _carCategoryFactoryMock);
    }

    [TestMethod]
    public void RegisterPickup_ShouldSaveRentalPickup()
    {
        // Act
        var result = _systemUnderTest.RegisterPickup(
            _rentalPickupMock.RegistrationNumber,
            _rentalPickupMock.SocialSecurityNumber,
            _rentalPickupMock.CarCategory.ToString(),
            _rentalPickupMock.PickupDateTimeInUtc,
            _rentalPickupMock.PickupMeterReadingInKm);

        // Assert - Check that SaveRentalPickup was called with ANY RentalPickup
        _carRegistrationRepositoryMock.Received(1).SaveRentalPickup(Arg.Any<RentalPickup>());

        // Optionally verify the properties of the saved object
        _carRegistrationRepositoryMock.Received(1).SaveRentalPickup(
            Arg.Is<RentalPickup>(rp =>
                rp.RegistrationNumber == _rentalPickupMock.RegistrationNumber &&
                rp.SocialSecurityNumber == _rentalPickupMock.SocialSecurityNumber));
    }

    [TestMethod]
    public void RegisterReturn_ShouldCalculateRentalPriceAndSaveRentalReturn()
    {
        // Arrange
        var returnDateTime = DateTime.Now.AddDays(3);
        var returnMeterReadingInKm = 1500;

        // Act
        _ = _systemUnderTest.RegisterReturn(_rentalPickupMock, returnDateTime, returnMeterReadingInKm);

        // Assert
        _carRegistrationRepositoryMock.Received(1).SaveRentalReturn(_rentalPickupMock.BookingNumber, Arg.Any<Domain.Entities.RentalReturn>());
        _carRegistrationRepositoryMock.Received(1).RemoveActiveRentalPickup(_rentalPickupMock.BookingNumber);
    }

    [TestMethod]
    public void RegisterReturn_ShouldCalculateCorrectRentalPrice()
    {
        // Arrange
        _carCategoryMock.CalculateRentalPrice(
            Arg.Any<int>(),
            Arg.Any<int>(),
            Arg.Any<decimal>(),
            Arg.Any<decimal>())
            .Returns(500m);

        var returnDateTime = DateTime.UtcNow.AddDays(3);
        var returnMeterReadingInKm = 1500;

        // Act
        var rentalPrice = _systemUnderTest.RegisterReturn(
            _rentalPickupMock,
            returnDateTime,
            returnMeterReadingInKm);

        // Assert
        Assert.AreEqual(500m, rentalPrice);
    }

    [TestMethod]
    public void Constructor_ShouldThrowArgumentNullException_WhenCarRegistrationRepositoryIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CarRegistrationUseCase(null!, _carCategoryFactoryMock));
    }

    [TestMethod]
    public void Constructor_ShouldThrowArgumentNullException_When_CarCategoryFactoryIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CarRegistrationUseCase(_carRegistrationRepositoryMock, null!));
    }
}