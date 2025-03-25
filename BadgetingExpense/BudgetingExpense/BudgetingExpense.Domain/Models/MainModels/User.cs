using System.ComponentModel.DataAnnotations;
namespace BudgetingExpense.Domain.Models.MainModels;

public class User
{
    [Key]
    public string Id { get; set; }
    [Length(2, 50)]
    [Required]
    public required string Name { get; set; }
    [Length(2, 50)]
    [Required]
    public required string Surname { get; set; }
    [EmailAddress]
    [Required]
    public required string Email { get; set; }
    [Length(8, 255)]
    public string? Password { get; set; }
    [Required]
    public DateTime RegisterDate { get; set; }
    public bool? Notifications { get; set; }
}
