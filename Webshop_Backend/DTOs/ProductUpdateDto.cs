using System.ComponentModel.DataAnnotations;

namespace Webshop_Backend.DTOs
{
public class ProductUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Url]
    public string ImageUrl { get; set; }

    [Required]
    public int CategoryId { get; set; }
}
}