using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Contracts.DTOs.Person
{
    public class FetchPersonDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public long RawPhoneNumber { get; set; }

        public string PhoneNumber => string.Concat("0", RawPhoneNumber);
        public required string Email { get; set; }
    }
}
