using System.ComponentModel.DataAnnotations;

namespace Unosoft.EFactoringManager.Users.Dto;

public class ChangePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; }

    [Required]
    public string NewPassword { get; set; }
}
