using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Authorization.Users
{
    public class FourDigitPasswordValidator : IPasswordValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(
            UserManager<User> manager,
            User user,
            string password)
        {
            if (!Regex.IsMatch(password, @"^\d{4}$"))
            {
                return Task.FromResult(
                    IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "InvalidPassword",
                            Description = "La contraseña debe tener exactamente 4 números"
                        }
                    )
                );
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
