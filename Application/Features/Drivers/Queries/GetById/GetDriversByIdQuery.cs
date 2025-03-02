using Domain.Logistic;

namespace Application.Features.Drivers.Queries.GetById;

public record GetDriversByIdQuery(
    int DriverId) : IRequest<IResult>;