
using Application.Dto.TripRequests;

namespace Application.Dto.Consignments;

public class ConsignmentDto
{
    public int ConsignmentId { get; set; }
    public string PartyName { get; set; }
    public string PartyContact { get; set; }
    public string GoodDescription { get; set; }
    public string TotalWeight { get; set; }
    public string LoadingPoint { get; set; }
    public string UnloadingPoint { get; set; }
    public ICollection<TripRequestDto> TripRequestDto { get; set; } = new List<TripRequestDto>();

}
