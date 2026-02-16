using CarRentalSystem.Domain.Interfaces;

namespace CarRentalSystem.Domain.Entities;

public sealed class SmallCar : ICarCategory
{
    public string Name => "Small Car";

    public decimal CalculateRentalPrice(int numberOfDays, int numberOfKm, decimal baseDayRental, decimal baseKmPrice)
    {
        return baseDayRental * numberOfDays;
    }
}
