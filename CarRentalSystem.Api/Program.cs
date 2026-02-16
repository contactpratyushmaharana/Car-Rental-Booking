using CarRentalSystem.Application.Factories;
using CarRentalSystem.Application.Interfaces;
using CarRentalSystem.Application.Usecases;
using CarRentalSystem.Infrastructure.Repositories;

bool _running = true;

//Ideally these dependencies should be injected using a DI container,
//but for simplicity, we are instantiating them directly here.
ICarRegistration _carRegistration = new CarRegistrationUseCase(new CarRegistrationRepository(), new CarCategoryFactory());

Console.WriteLine("=============================================================");
Console.WriteLine("           WELCOME TO CAR RENTAL MANAGEMENT SYSTEM           ");
Console.WriteLine("=============================================================");

while (_running)
{
    Console.WriteLine("\nPlease select an option:");
    Console.WriteLine("1. Register a car pickup");
    Console.WriteLine("2. Register a car return");
    Console.WriteLine("3. Exit");
    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            Console.WriteLine("\n--- Car Pickup Registration ---");

            Console.Write("Enter car registration number: ");
            var registrationNumber = Console.ReadLine();

            Console.Write("Enter customer's social security number: ");
            var socialSecurityNumber = Console.ReadLine();

            Console.WriteLine("Select car category:");
            Console.WriteLine("1. Small Car");
            Console.WriteLine("2. Combi");
            Console.WriteLine("3. Truck");
            var categoryInput = Console.ReadLine();

            Console.WriteLine("Enter pickup date and time in (yyyy-MM-dd HH:mm): ");
            var pickupDateTimeInput = Console.ReadLine();

            Console.WriteLine("Enter pickup meter reading in km: ");
            var pickupMeterReading = Console.ReadLine();

            var bookingNumber = _carRegistration.RegisterPickup(
                registrationNumber, 
                socialSecurityNumber,
                categoryInput, 
                DateTime.Parse(pickupDateTimeInput), 
                int.Parse(pickupMeterReading));

            Console.WriteLine($"Booking registered successfully! Your booking number is: {bookingNumber.BookingNumber}");
            break;
        case "2":
            Console.WriteLine("\n--- Car Return Registration ---");

            Console.WriteLine("Enter booking number: ");
            var bookingNumberInput = Console.ReadLine();

            Console.WriteLine("Enter return date and time in (yyyy-MM-dd HH:mm): ");
            var returnDateTimeInput = Console.ReadLine();

            Console.WriteLine("Enter return meter reading in km: ");
            var returnMeterReading = Console.ReadLine();

            var totalRentalPrice = _carRegistration.RegisterReturn(
                CarRegistrationRepository.ActiveRentals[Guid.Parse(bookingNumberInput)], 
                DateTime.Parse(returnDateTimeInput), 
                int.Parse(returnMeterReading));

            Console.WriteLine("Car return registered successfully! Total rental price: " + totalRentalPrice);
            break;
        case "3":
            _running = false;
            break;
    }
}





