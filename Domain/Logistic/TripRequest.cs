
using Domain.Logistic.Enum;
using Microsoft.AspNetCore.Http;

namespace Domain.Logistic;

public class TripRequest
{
    public int Id { get; set; }
    public int? TripId { get; set; }
    public Trip Trip { get; set; } = null;
    public int? ConsignmentId { get; set; }
    public Consignment Consignment { get; set; } = null;
    public int? VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null;
    public int? DriverId { get; set; }
    public Driver Driver { get; set; } = null;
    public int? FreightId { get; set; }
    public Freight Freight { get; set; } = null;
    //[NotMapped]
    //public IFormFile FuelReceipt { get; set; }
    public string FuelReceiptPath { get; set; }
    public string RefuelStation { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }

    public bool IsActive { get; set; } = true;


}
