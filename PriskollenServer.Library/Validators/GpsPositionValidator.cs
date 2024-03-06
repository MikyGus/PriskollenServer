using ErrorOr;
using PriskollenServer.Library.Contracts;
using PriskollenServer.Library.ServiceErrors;

namespace PriskollenServer.Library.Validators;
public class GpsPositionValidator : IGpsPositionValidator
{
    public const int MinLatitude = -90;
    public const int MaxLatitude = 90;
    public const int MinLongitude = -180;
    public const int MaxLongitude = 180;

    public bool IsValid(GpsRequest gpsRequest, out List<Error> errors)
    {
        errors = [];
        if (gpsRequest.Latitude is < MinLatitude or > MaxLatitude)
        {
            errors.Add(Errors.GpsPosition.InvalidLatitude);
        }

        if (gpsRequest.Longitude is < MinLongitude or > MaxLongitude)
        {
            errors.Add(Errors.GpsPosition.InvalidLongitude);
        }

        if (gpsRequest.Latitude is null ^ gpsRequest.Longitude is null)
        {
            errors.Add(Errors.GpsPosition.MissingLatitudeOrLongitude);
        }

        return errors.Count == 0;
    }

    public ErrorOr<GpsRequest> Validate(GpsRequest request) => IsValid(request, out List<Error> errors) ? request : errors;
}