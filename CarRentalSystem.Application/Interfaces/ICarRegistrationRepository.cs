using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Application.Interfaces;

public interface ICarRegistrationRepository
{
    void SaveRentalPickup(RentalPickup rentalPickup);

    void RemoveActiveRentalPickup(Guid bookingNumber);

    void SaveRentalReturn(Guid bookingNumber, RentalReturn rentalReturn);
}
