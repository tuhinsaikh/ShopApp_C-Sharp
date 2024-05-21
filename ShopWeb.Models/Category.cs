using System.ComponentModel.DataAnnotations;

namespace ShopWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [Range(0,100, ErrorMessage ="Display order must between 0 to 100")]
        public int DisplayOrder { get; set; }

    }
}
