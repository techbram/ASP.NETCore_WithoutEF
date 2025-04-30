using System.ComponentModel.DataAnnotations;

namespace ASP.NETCore_WithoutEF.App.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }   
        public string? CreatedDate { get; set; }

        [Range(1,100)]
        public int  DisplayOrder {  get; set; }
    }
}
