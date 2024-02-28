using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Person
    {
		[Key]
		public int Id { get; set; }
		[MaxLength(1024)]
		public required string Firstname { get; set; }
        [MaxLength(1024)]
        public required string Lastname { get; set; }
		[StringLength(10)]
		public required string DateOfBirth { get; set; }
		[StringLength(11)]
		public required string PhoneNumber { get; set; }

		[MaxLength(1024)]
		public required string Email { get; set; }
    }
}