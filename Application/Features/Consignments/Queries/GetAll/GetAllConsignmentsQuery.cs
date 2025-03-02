namespace Application.Features.Consignments.Queries.GetAll;

public record GetAllConsignmentsQuery(int PageNumber , int PageSize) : IRequest<IResult>;

