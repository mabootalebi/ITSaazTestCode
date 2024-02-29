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
		public required DateOnly DateOfBirth { get; set; }
		public long PhoneNumber { get; set; }

		[MaxLength(1024)]
		public required string Email { get; set; }
    }
}