

using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.Freights;
using Application.Dto.Trips;
using Application.Dto.Vehicles;
using Domain.Logistic.Enum;

namespace Application.Dto.TripRequests;

public class TripRequestDto
{
    public int Id { get; set; }
    //public TripDto TripDto { get; set; }
    //public int ConsignmentId { get; set; }
    public ConsignmentDto ConsignmentDto { get; set; }
    //public int VehicleId { get; set; }
    public VehicleDto VehicleDto { get; set; }
    //public int DriverId { get; set; }
    //public int TripId { get; set; }
    public TripDto TripDto { get; set; }
    public DriverDto DriverDto { get; set; }
    public FreightDto FreightDto { get; set; }
    //public IFormFile FuelReceipt { get; set; }
    public string FuelReceiptPath { get; set; }
    public string RefuelStation { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
}

