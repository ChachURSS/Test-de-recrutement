using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AngularWithASP.Server.Models
{
    /// <summary>
    /// Represents a garage where cars can be parked.
    /// </summary>
    public class Garage
    {
        /// <summary>
        /// The unique identifier of the garage.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the garage.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The list of cars parked in the garage.
        /// </summary>
        [JsonIgnore]
        public ICollection<Car>? Cars { get; set; }
        /// <summary>
        /// The address of the garage.
        /// </summary>
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// The city where the garage is located.
        /// </summary>
        public string City { get; set; } = string.Empty;
    }
}
