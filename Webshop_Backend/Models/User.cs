namespace Webshop_Backend.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    public string Role { get; set; } = "User";
    
    // Dodajte ovo ako već imate Cart model
    public Cart Cart { get; set; }
}
