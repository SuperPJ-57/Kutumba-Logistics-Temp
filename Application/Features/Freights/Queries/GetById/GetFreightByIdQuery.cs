namespace Application.Features.Freights.Queries.GetById;

public record GetFreightByIdQuery(int FreightId) : IRequest<IResult>;
