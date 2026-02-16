using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Domain.Entities;

namespace CarRentalSystem.Infrastructure.Repositories;

public class CarRegistrationRepository : ICarRegistrationRepository
{
    private static readonly Dictionary<Guid, RentalPickup> _activeRentals = [];
    private static readonly Dictionary<Guid, RentalReturn> _completedRentals = [];

    public static IReadOnlyDictionary<Guid, RentalPickup> ActiveRentals => _activeRentals;

    public static IReadOnlyDictionary<Guid, RentalReturn> CompletedRentals => _completedRentals;

    public void SaveRentalReturn(Guid bookingNumber, RentalReturn rentalReturn)
    {
        if (!_activeRentals.ContainsKey(bookingNumber))
        {
            throw new InvalidOperationException($"No active rental found with booking number {bookingNumber}");
        }

        _completedRentals[bookingNumber] = rentalReturn;
    }

    public void RemoveActiveRentalPickup(Guid bookingNumber)
    {
        if (!_activeRentals.ContainsKey(bookingNumber))
        {
            throw new InvalidOperationException($"No active rental found with booking number {bookingNumber}");
        }

        _activeRentals.Remove(bookingNumber);
    }

    public void SaveRentalPickup(RentalPickup rentalPickup)
    {
        ArgumentNullException.ThrowIfNull(rentalPickup);

        if (_activeRentals.ContainsKey(rentalPickup.BookingNumber))
        {
            throw new InvalidOperationException(
                $"A Booking with this number {rentalPickup.BookingNumber} already exists");
        }

        _activeRentals[rentalPickup.BookingNumber] = rentalPickup;
    }
}