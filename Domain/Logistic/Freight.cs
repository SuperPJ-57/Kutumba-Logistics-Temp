
using System.Text.Json.Serialization;
using Domain.Logistic.Enum;

namespace Domain.Logistic;

public class Freight
{
    public int FreightId { get; set; }
    public string FreightRate { get; set; }
    public string AdvancedPaid { get; set; }
    public string RemainingAmount { get; set; }
    public PaymentMode PaymentMode { get; set; }

    public bool IsActive { get; set; } = true;

    [JsonIgnore]
    public ICollection<TripRequest> TripRequest { get; set; } = new List<TripRequest>();
}
