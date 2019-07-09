using System.ComponentModel.DataAnnotations;

namespace ZwajApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

[Required]
        [StringLength(10,MinimumLength=6,ErrorMessage="يجب ان تكون كلمة المرور اقل من 10 واكبر 6")]
        public string Password { get; set; }
    }
}