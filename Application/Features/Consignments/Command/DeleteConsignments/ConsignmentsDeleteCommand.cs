namespace Application.Features.Consignments.Command.DeleteConsignment;

public record ConsignmentsDeleteCommand(int ConsignmentId) : IRequest<IResult>;

