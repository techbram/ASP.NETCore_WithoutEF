using ASP.NETCore_WithoutEF.App.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.ViewModel
{
    public class RegisterVM
    {
        [Required]
        [DataType(DataType.Password)]
        //[Compare(nameof(User.PasswordHash), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }        
        public ApplicationUser User { get; set; }
        
        
        //[ValidateNever]
        //public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}
