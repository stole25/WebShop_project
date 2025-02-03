using System.ComponentModel.DataAnnotations;

namespace Webshop_Backend.DTOs;

public class CategoryUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}