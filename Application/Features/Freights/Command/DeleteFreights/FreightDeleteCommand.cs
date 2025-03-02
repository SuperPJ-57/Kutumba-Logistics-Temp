namespace Application.Features.Freights.Command.DeleteFreights;

public record FreightDeleteCommand(int FreightId) : IRequest<IResult>;
