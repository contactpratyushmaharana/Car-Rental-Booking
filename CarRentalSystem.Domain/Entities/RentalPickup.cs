using CarRentalSystem.Domain.Interfaces;

namespace CarRentalSystem.Domain.Entities;

public sealed class RentalPickup(Guid bookingNumber, string registrationNumber, string socialSecurityNumber, 
    ICarCategory carCategory, DateTime pickupDateTime, int pickupMeterReadingInKm)
{
    public Guid BookingNumber { get; } = bookingNumber != Guid.Empty ? 
        bookingNumber : throw new ArgumentException("Invalid booking number", nameof(bookingNumber));

    public string RegistrationNumber { get; } = !string.IsNullOrWhiteSpace(registrationNumber) ? 
        registrationNumber : throw new ArgumentException("Invalid registration number", nameof(registrationNumber));

    public string SocialSecurityNumber { get; } = !string.IsNullOrWhiteSpace(socialSecurityNumber) ? 
        socialSecurityNumber : throw new ArgumentException("Invalid social security number", nameof(socialSecurityNumber));

    public ICarCategory CarCategory { get; } = carCategory ?? 
        throw new ArgumentNullException("Invalid car category", nameof(carCategory));

    public DateTime PickupDateTimeInUtc { get; } =
        pickupDateTime != default && pickupDateTime.ToUniversalTime() >= DateTime.UtcNow.AddMinutes(-1)
            ? pickupDateTime.ToUniversalTime()
            : throw new ArgumentException("Invalid Pickup date and time", nameof(pickupDateTime));

    public int PickupMeterReadingInKm { get; } = pickupMeterReadingInKm >= 0 ?
        pickupMeterReadingInKm : throw new ArgumentOutOfRangeException("Invalid current meter reading", nameof(pickupMeterReadingInKm));
}
