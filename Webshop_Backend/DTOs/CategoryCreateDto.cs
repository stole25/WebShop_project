using System.ComponentModel.DataAnnotations;

namespace Webshop_Backend.DTOs;

public class CategoryCreateDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}