namespace Application.Features.Freights.Queries.GetAll;

public record GetAllFreightQuery(int PageNumber,int PageSize) : IRequest<IResult>;
