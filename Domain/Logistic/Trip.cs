
using System.Text.Json.Serialization;
using Domain.Logistic.Enum;

namespace Domain.Logistic;

public class Trip
{
    public int TripId { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime DeliveryDate { get; set; } 
    public string TripAllowance { get; set; }
    public string MaintenanceFee { get; set; }
    public string DriverAllownace { get; set; }
    public Status Status { get; set; } = Status.Inactive;

    public bool IsActive { get; set; } = true;

    [JsonIgnore]
    public ICollection<TripRequest> TripRequest { get; set; } = new List<TripRequest>();
}
