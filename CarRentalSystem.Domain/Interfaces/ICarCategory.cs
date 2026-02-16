namespace CarRentalSystem.Domain.Interfaces;

public interface ICarCategory
{
    string Name { get; }

    decimal CalculateRentalPrice(int numberOfDays, int numberOfKm, decimal baseDayRental, decimal baseKmPrice);
}
