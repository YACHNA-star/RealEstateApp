// Models/Property.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateApp.Models
{
    public class Property
    {
        public int Id { get; set; }

        [Required]
        public string Place { get; set; }

        [Required]
        public double Area { get; set; }

        [Required]
        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public string? NearbyHospitals { get; set; }
        public string? NearbyColleges { get; set; }
        public int Likes { get; set; }

        //[ForeignKey("Seller")]
        public int SellerId { get; set; }

        public User Seller { get; set; }
    }
}
