

namespace Application.Dto.TransportationOrder
{
    public class TransportationOrderDto
    {
        public int Id { get; set; }
        public string AssignVehicle { get; set; }
        public string ConsignmentPriority { get; set; }
        public string LoadingPoint { get; set; }
        public string UnloadingPoint { get; set; }
        public string ClientName { get; set; }
        public string ClientContact { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal DriverAllowance { get; set; }
        public decimal FreightRate { get; set; }
        public decimal MaintenanceFee { get; set; }
        public decimal TripAllowance { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string DocumentPath { get; set; }
    }
}
