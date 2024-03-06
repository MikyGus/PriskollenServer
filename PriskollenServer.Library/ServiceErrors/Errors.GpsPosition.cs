using ErrorOr;
using PriskollenServer.Library.Validators;

namespace PriskollenServer.Library.ServiceErrors;
public static partial class Errors
{
    public static class GpsPosition
    {
        public static Error MissingLatitudeOrLongitude => Error.Validation(
            "GpsPosition.MissingLatitudeOrLongitude",
            $"Both Latitude and Longitude is needed for a valid position");

        public static Error InvalidLatitude => Error.Validation(
            "Store.InvalidLatitude",
            $"Latitude must be between {GpsPositionValidator.MinLatitude} " +
            $"and {GpsPositionValidator.MaxLatitude} inclusive to be considered valid.");

        public static Error InvalidLongitude => Error.Validation(
            "Store.InvalidLongitude",
            $"Longitude must be between {GpsPositionValidator.MinLongitude} " +
            $"and {GpsPositionValidator.MaxLongitude} inclusive to be considered valid.");
    }
}