namespace Tutorial5.Models.DTOs;
using System.ComponentModel.DataAnnotations;
public class DeleteAnimal
{
    [Required]
    [MinLength(3)]
    [MaxLength(200)]
    public string Name { get; set; }
    [MinLength(3)]
    [MaxLength(200)]
    public string? Description { get; set; }
    [MinLength(3)]
    [MaxLength(200)]
    public string? Category { get; set; }
    [MinLength(3)]
    [MaxLength(200)]
    public string? Area { get; set; }
}