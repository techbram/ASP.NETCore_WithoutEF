using System.ComponentModel.DataAnnotations;

namespace ASP.NETCore_WithoutEF.App.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Display(Name = "Address")]
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
