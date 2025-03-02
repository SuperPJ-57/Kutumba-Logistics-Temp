using Application.Dto.Consignments;
using Application.Dto.Drivers;
using Application.Dto.TripRequests;
using Domain.Logistic;
using Domain.Logistic.Enum;
using Microsoft.Identity.Client;

namespace Application.Dto.Trips;

public class TripDto
{
    public int TripId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string TripAllowance { get; set; }
    public string MaintenanceFee { get; set; }
    public string DriverAllownace { get; set; }
    public Status Status { get; set; }
    public ICollection<TripRequestDto> TripRequestDto { get; set; } = new List<TripRequestDto>();
}
