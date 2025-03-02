using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.Freights;
using Application.Dto.TripRequests;
using Application.Dto.Trips;
using Application.Dto.Vehicles;
using Application.Features.Consignments.Queries.GetAll;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;




namespace Application.Features.ConsignmentDetails.Queries.GetAll;

public class GetAllConsignmentsQueryHandler : IRequestHandler<GetAllConsignmentsQuery, IResult>
{
    private readonly IConsignmentRepository _consignmentDetails;
    private readonly ILogger<GetAllConsignmentsQuery> _logger;
    public GetAllConsignmentsQueryHandler(IConsignmentRepository consignmentDetails, ILogger<GetAllConsignmentsQuery> logger)
    {
        _consignmentDetails = consignmentDetails;
        _logger = logger;
    }

    public async Task<IResult> Handle(GetAllConsignmentsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{FunctionName} received a request to retrieve all consignment.",
                nameof(GetAllConsignmentsQueryHandler));

        try
        {
            var totalCount = _consignmentDetails.Queryable.Count();

            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            var consignments = await _consignmentDetails.Queryable                
                .Include(c => c.TripRequest)
                    .ThenInclude(tr => tr.Vehicle)
                .Include(c => c.TripRequest)
                    .ThenInclude(tr => tr.Driver)
                .Include(c => c.TripRequest)
                    .ThenInclude(tr => tr.Trip)
                .Include(c => c.TripRequest)
                    .ThenInclude(tr => tr.Freight)
                .OrderBy(c => c.PartyName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var consignmentDtos = consignments.Select(c => new ConsignmentDto
            {
                ConsignmentId = c.ConsignmentId,
                PartyName = c.PartyName,
                PartyContact = c.PartyContact,
                GoodDescription = c.GoodDescription,
                TotalWeight = c.TotalWeight,
                LoadingPoint = c.LoadingPoint,
                UnloadingPoint = c.UnloadingPoint,
                TripRequestDto = c.TripRequest.Select(tr => new TripRequestDto
                {
                    Id = tr.Id,
                    VehicleDto = tr.Vehicle != null ? new VehicleDto
                    {
                        VehicleId = tr.Vehicle.VehicleId,
                        VehicleCapacity = tr.Vehicle.VehicleCapacity,
                        VehicleImagePath = tr.Vehicle.VehicleImagePath,
                        VehicleName = tr.Vehicle.VehicleName,
                        VehicleOwner = tr.Vehicle.VehicleOwner,
                        VehicleType = tr.Vehicle.VehicleType,
                        VehicleVolume = tr.Vehicle.VehicleVolume,
                        VehicleNumber = tr.Vehicle.VehicleNumber,
                        Status = tr.Vehicle.Status
                    } : null,

                    DriverDto = tr.Driver != null ? new DriverDto
                    {
                        DriverId = tr.Driver.DriverId,
                        FirstName = tr.Driver.FirstName,
                        LastName = tr.Driver.LastName,
                        Contact = tr.Driver.Contact,
                        LicenseNumber = tr.Driver.LicenseNumber,
                        ProfileImagePath = tr.Driver.ProfileImagePath,
                        Status = tr.Driver.Status,
                    } : null,

                    TripDto = tr.Trip != null ? new TripDto
                    {
                        TripId = tr.Trip.TripId,
                        TripAllowance = tr.Trip.TripAllowance,
                        DriverAllownace = tr.Trip.DriverAllownace,
                        MaintenanceFee = tr.Trip.MaintenanceFee,
                        StartDate = tr.Trip.StartDate,
                        Status = tr.Trip.Status,
                        DeliveryDate = tr.Trip.DeliveryDate,
                    } : null,

                    FreightDto = tr.Freight != null ? new FreightDto
                    {
                        FreightId = tr.Freight.FreightId,
                        AdvancedPaid = tr.Freight.AdvancedPaid,
                        RemainingAmount = tr.Freight.RemainingAmount,
                        FreightRate = tr.Freight.FreightRate
                    } : null,
                    
                    
                    FuelReceiptPath = tr.FuelReceiptPath,
                    RefuelStation = tr.RefuelStation,
                    ApprovalStatus = tr.ApprovalStatus
                }).ToList()

            }).ToList();

            _logger.LogInformation("{FunctionName} successfully retrieved all consignment. Count: {@Count}",
                nameof(GetAllConsignmentsQueryHandler), totalCount);

            return Results.Ok(new
            {
                Consignment = consignmentDtos,
                meta = new
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                }
            });
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "{FunctionName} error occurred while retrieving  all the consignment.",
                nameof(GetAllConsignmentsQueryHandler));

            return Results.BadRequest(new
            {
                Message = "An error occurred while retrieving accounts. Please try again later."
            });
        }
    }
}

