using CarRentalSystem.Domain.Interfaces;

namespace CarRentalSystem.Domain.Entities;

public sealed class Combi : ICarCategory
{
    public string Name => "Combi";

    public decimal CalculateRentalPrice(int numberOfDays, int numberOfKm, decimal baseDayRental, decimal baseKmPrice)
    {
        return (baseDayRental * numberOfDays * 1.3m) + (baseKmPrice * numberOfKm);
    }
}
