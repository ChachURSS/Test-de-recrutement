namespace AngularWithASP.Server.Models
{
    public class Garage
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Car>? Cars { get; set; }
    }
}
