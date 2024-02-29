
using System.ComponentModel.DataAnnotations;

namespace Contracts.DTOs.Person
{
    public class CreatePersonDto
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^(\\+989[0-9]{9})|(00989[0-9]{9}|(09[0-9]{9}))$", ErrorMessage = "Invalid Phone Number")]
        public required string PhoneNumber { get; set; }

        public long NormalizedPhonNumber => long.Parse(PhoneNumber.Replace("+98", "0").Replace("0098","0"));

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }
    }
}
