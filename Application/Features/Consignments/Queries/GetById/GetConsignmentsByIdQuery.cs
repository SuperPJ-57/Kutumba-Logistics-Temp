namespace Application.Features.Consignments.Queries.GetById;

public record GetConsignmentsByIdQuery(int ConsignmentId) : IRequest<IResult>;
