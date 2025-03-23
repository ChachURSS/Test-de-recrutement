using System.ComponentModel.DataAnnotations;

namespace AngularWithASP.Server.Models
{
    /// <summary>
    /// Represents a car.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// The unique identifier of the car.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The brand of the car.
        /// </summary>
        [Required]
        public string Brand { get; set; } = string.Empty;
        /// <summary>
        /// The model of the car.
        /// </summary>
        [Required]
        public string Model { get; set; } = string.Empty;
        /// <summary>
        /// The ID of the garage where the car is parked.
        /// </summary>
        public int? GarageId { get; set; }
        /// <summary>
        /// The garage where the car is parked.
        /// </summary>
        public Garage? Garage { get; set; }
        /// <summary>
        /// The color of the car.
        /// </summary>
        public string Color { get; set; } = string.Empty;
    }
}
