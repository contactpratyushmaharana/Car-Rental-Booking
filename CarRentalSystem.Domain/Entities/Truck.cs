using CarRentalSystem.Domain.Interfaces;

namespace CarRentalSystem.Domain.Entities;

public sealed class Truck : ICarCategory
{
    public string Name => "Truck";

    public decimal CalculateRentalPrice(int numberOfDays, int numberOfKm, decimal baseDayRental, decimal baseKmPrice)
    {
        return (baseDayRental * numberOfDays * 1.5m) + (baseKmPrice * numberOfKm * 1.5m);
    }
}