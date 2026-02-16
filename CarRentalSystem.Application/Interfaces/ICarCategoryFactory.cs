using CarRentalSystem.Domain.Interfaces;

namespace CarRentalSystem.Application.Interfaces;

public interface ICarCategoryFactory
{
    ICarCategory CreateCategory(string categoryType);
}