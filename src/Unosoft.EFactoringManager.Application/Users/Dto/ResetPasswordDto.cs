using System.ComponentModel.DataAnnotations;

namespace Unosoft.EFactoringManager.Users.Dto;

public class ResetPasswordDto
{
       

    [Required]
    public long UserId { get; set; }

  
    public string NewPassword { get; set; }
}
