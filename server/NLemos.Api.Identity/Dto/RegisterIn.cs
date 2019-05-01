using System.ComponentModel.DataAnnotations;

namespace NLemos.Api.Identity.Dto
{
    public class RegisterIn
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, ErrorMessage = "Name should have less than 255 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(255, ErrorMessage = "Email address should have less than 255 characters")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password should have at least 6 characters")]
        [MaxLength(30, ErrorMessage = "Password should have less than 30 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}