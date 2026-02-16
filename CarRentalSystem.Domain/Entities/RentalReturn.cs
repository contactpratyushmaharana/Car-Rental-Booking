namespace CarRentalSystem.Domain.Entities;

public sealed class RentalReturn(RentalPickup rentalPickup, DateTime returnDateTime, int returnMeterReadingInKm)
{
    public RentalPickup RentalPickup { get; } = rentalPickup ??
        throw new ArgumentNullException("Return must have a pickup associated with it", nameof(rentalPickup));

    public Guid BookingNumber { get; } = rentalPickup.BookingNumber;

    public DateTime ReturnDateTimeInUtc { get; } = returnDateTime != default && returnDateTime.ToUniversalTime() >= rentalPickup.PickupDateTimeInUtc
            ? returnDateTime.ToUniversalTime()
            : throw new ArgumentException("Return date and time must be on or after the pickup date and time", nameof(returnDateTime));

    public int ReturnMeterReadingInKm { get; } = returnMeterReadingInKm >= rentalPickup.PickupMeterReadingInKm ?
        returnMeterReadingInKm : throw new ArgumentOutOfRangeException("Invalid current meter reading", nameof(returnMeterReadingInKm));

    public int GetRentalPeriodInDays()
    {
        var timeSpan = ReturnDateTimeInUtc - rentalPickup.PickupDateTimeInUtc;

        // If the return day is the same as the pickup day same day then we can count as 1 day minimum
        // If there are any hours or minutes left beyond full days then we can round up to the next day
        int days = timeSpan.Days;
        if (timeSpan.Hours > 0 || timeSpan.Minutes > 0 || timeSpan.Seconds > 0)
            days++;

        return Math.Max(1, days);
    }

    public int GetNumberOfKms()
    {
        return ReturnMeterReadingInKm - rentalPickup.PickupMeterReadingInKm;
    }
}
