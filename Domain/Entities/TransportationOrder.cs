
namespace Domain.Entities
{
    public class TransportationOrder
    {
        public int Id { get; set; }
        public string AssignVehicle { get; set; }
        public string ConsignmentPriority { get; set; }
        public string LoadingPoint { get; set; }
        public string UnloadingPoint { get; set; }
        public string ClientName    { get; set; }
        public string ClientContact { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeliveryDate { get; set; }

        [Precision(18, 2)]
        public decimal DriverAllowance { get; set; }

        [Precision(18, 2)]
        public decimal FreightRate { get; set; }

        [Precision(18, 2)]
        public decimal MaintenanceFee { get; set; }

        [Precision(18, 2)]
        public decimal TripAllowance { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string DocumentPath { get; set; }
    }
}
