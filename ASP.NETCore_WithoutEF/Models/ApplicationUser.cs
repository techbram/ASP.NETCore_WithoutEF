using System.Numerics;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace ASP.NETCore_WithoutEF.Models
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

        public int? CompanyId {  get; set; }

        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company Company { get; set; }

        //public string? NormalizedUserName { get; set; }
        //public byte EmailConfirmed { get; set; }
        
    }
}
