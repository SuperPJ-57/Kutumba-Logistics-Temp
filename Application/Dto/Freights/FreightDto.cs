using Application.Dto.TripRequests;
using Domain.Logistic.Enum;

namespace Application.Dto.Freights;

public class FreightDto
{
    public int FreightId { get; set; }
    public string FreightRate { get; set; }
    public string AdvancedPaid { get; set; }
    public string RemainingAmount { get; set; }
    public PaymentMode PaymentMode { get; set; }
    public ICollection<TripRequestDto> TripRequestDto { get; set; } = new List<TripRequestDto>();
}
