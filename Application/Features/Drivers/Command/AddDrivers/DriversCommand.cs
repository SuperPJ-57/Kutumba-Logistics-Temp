
using Domain.Logistic;
using Domain.Logistic.Enum;

namespace Application.Features.Drivers.Command.AddDrivers;

public record DriversCommand(
    int DriverId,
    string FirstName,
    string LastName,
    string Contact,
    string LicenseNumber,
    DateOnly DateOfBirth,
    //IFormFile ProfileImage,
    Status Status) : IRequest<IResult>;
