using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Domain.Entities;
using CarRentalSystem.Domain.Interfaces;

namespace CarRentalSystem.Application.Factories;

public sealed class CarCategoryFactory : ICarCategoryFactory
{
    public ICarCategory CreateCategory(string categoryType)
    {
        if (string.IsNullOrWhiteSpace(categoryType))
        {
            throw new ArgumentException("Category type cannot be null or empty", nameof(categoryType));
        }

        return categoryType switch
        {
            "1" => new SmallCar(),
            "2" => new Combi(),
            "3" => new Truck(),
            _ => throw new ArgumentException($"Invalid category selection: '{categoryType}'")
        };
    }
}