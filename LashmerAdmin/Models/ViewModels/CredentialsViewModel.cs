using System.ComponentModel.DataAnnotations;

namespace LashmerAdmin.Models.ViewModels
{
  public class CredentialsViewModel
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
  }
}
