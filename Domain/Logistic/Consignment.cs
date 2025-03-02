

using System.Text.Json.Serialization;

namespace Domain.Logistic;

public class Consignment
{
    public int ConsignmentId { get; set; }
    public string PartyName { get; set; }
    public string PartyContact { get; set; }
    public string GoodDescription { get; set; }
    public string TotalWeight { get; set; }
    public string LoadingPoint { get; set; }
    public string UnloadingPoint { get; set; }
    public bool IsActive { get; set; } = true;
    [JsonIgnore]
    public ICollection<TripRequest> TripRequest { get; set; } = new List<TripRequest>();
}
