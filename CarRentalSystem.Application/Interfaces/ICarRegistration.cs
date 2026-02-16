using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Interfaces;

namespace CarRentalSystem.Application.Interfaces;

public interface ICarRegistration
{
    RentalPickup RegisterPickup(
        string registrationNumber, 
        string socialSecurityNumber,
        string categoryType, 
        DateTime pickupDateTime, 
        int pickupMeterReadingInKm);

    decimal RegisterReturn(RentalPickup rentalPickup, DateTime returnDateTime, int returnMeterReadingInKm);
}
