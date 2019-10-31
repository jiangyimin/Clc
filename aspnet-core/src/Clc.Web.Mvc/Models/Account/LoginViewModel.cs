using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace Clc.Web.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        public string UsernameOrEmailAddress { get; set; }

        [Required]
        [DisableAuditing]
        public string Password { get; set; }

        [DisableAuditing]
        public string VerifyCode { get; set; }

        public bool RememberMe { get; set; }
    }
}
