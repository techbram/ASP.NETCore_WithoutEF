using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using App.Models;


namespace ASP.NETCore_WithoutEF.App.Models
{
    public class ApplicationUser 
    {
        [Key]
        public string Id { get; set; }
               
        public string? UserName { get; set; }
                
        public string? PasswordHash { get; set; }
        
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public string Name { get; set; }

        public string? StreetAddress { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

        public byte LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company? CompanyRef { get; set; }

        [ForeignKey("RoleId")]
        [ValidateNever]
        public Role? RoleRef { get; set; }

        public string? RoleId { get; set; } 

    }
}
