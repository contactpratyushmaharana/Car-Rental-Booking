using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Application.Usecases;

public class CarRegistrationUseCase(ICarRegistrationRepository carRegistrationRepository, ICarCategoryFactory carCategoryFactory) : ICarRegistration
{
    //Ideally these base prices should be configurable, but for simplicity, we are using constants here.
    private const decimal BASE_DAY_RENTAL = 100m;
    private const decimal BASE_KM_PRICE = 2.0m;

    private readonly ICarRegistrationRepository _carRegistrationRepository = carRegistrationRepository ?? 
        throw new ArgumentNullException(nameof(carRegistrationRepository));
    private readonly ICarCategoryFactory _carCategoryFactory = carCategoryFactory ?? 
        throw new ArgumentNullException(nameof(carCategoryFactory));

    public RentalPickup RegisterPickup(
        string registrationNumber, 
        string socialSecurityNumber,
        string categoryType,
        DateTime pickupDateTime, 
        int pickupMeterReadingInKm)
    {
        var carCategory = _carCategoryFactory.CreateCategory(categoryType);
        var bookingNumber = Guid.NewGuid();

        var rentalPickup = new RentalPickup(
            bookingNumber, 
            registrationNumber, 
            socialSecurityNumber, 
            carCategory,
            pickupDateTime, 
            pickupMeterReadingInKm);

        _carRegistrationRepository.SaveRentalPickup(rentalPickup);

        return rentalPickup;
    }

    public decimal RegisterReturn(RentalPickup rentalPickup, DateTime returnDateTime, int returnMeterReadingInKm)
    {
        var rentalReturn = new RentalReturn(rentalPickup, returnDateTime, returnMeterReadingInKm);

        var rentalPrice = rentalPickup.CarCategory.CalculateRentalPrice(
            rentalReturn.GetRentalPeriodInDays(),
            rentalReturn.GetNumberOfKms(),
            BASE_DAY_RENTAL,
            BASE_KM_PRICE);

        _carRegistrationRepository.SaveRentalReturn(rentalPickup.BookingNumber, rentalReturn);
        _carRegistrationRepository.RemoveActiveRentalPickup(rentalPickup.BookingNumber);

        return rentalPrice;
    }
}