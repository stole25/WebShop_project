using System.ComponentModel.DataAnnotations;

namespace Webshop_Backend.DTOs
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Naziv je obavezan")]
        [StringLength(100, ErrorMessage = "Naziv ne smije biti duži od 100 znakova")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Opis ne smije biti duži od 500 znakova")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cijena je obavezna")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cijena mora biti veća od 0")]
        public decimal Price { get; set; }

        [Url(ErrorMessage = "Neispravan URL slike")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Kategorija je obavezna")]
        public int CategoryId { get; set; }
    }
}