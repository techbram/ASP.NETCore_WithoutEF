using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ASP.NETCore_WithoutEF.App.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]        
        public string ProductName { get; set; }  
        
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? CategoryRef { get; set; }

        [Required]
        public double Price { get; set; }

        [ValidateNever]
        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }

        [ValidateNever]        
        public string? OldImageUrl { get; set; } = string.Empty;


        public string? ProductDetail { get; set; }


        [Required]
        [Range(1, 1000)]
        public double DiscountPrice { get; set; }

       
        [Required]
        public string? DateAdded { get; set; }


        [Required]
        public string ProductNumber { get; set; }


        [Required]
        public string? Originated { get; set; }


        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }


        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }
    }
}
