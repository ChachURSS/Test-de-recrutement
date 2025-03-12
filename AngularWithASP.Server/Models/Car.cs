namespace AngularWithASP.Server.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int? GarageId { get; set; }
        public Garage? Garage { get; set; }
    }
}
