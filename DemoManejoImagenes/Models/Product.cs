using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoManejoImagenes.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo nombre el requerido")]
        [StringLength(50)]
        public string Name { get; set; } 
        public string Description { get; set; }
        public string? UrlImage { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
