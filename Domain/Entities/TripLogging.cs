namespace Domain.Entities
{
    public class TripLogging
    {
        public int Id { get; set; }
        public string LoadingPoint { get; set; }
        public string ReFilledFuelStation { get; set; }
        public string Weight { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImagePath { get; set; }
    }
}
