using Domain.Logistic.Enum;

namespace Application.Features.Drivers.Command.UpdateDrivers;

public record DriversUpdateCommand(
    int DriverId,
    string FirstName,
    string LastName,
    string Contact, 
    string LicenseNumber,
    DateTime DateOfBirth,
    IFormFile ProfileImage,
    Status Status) : IRequest<IResult>;

