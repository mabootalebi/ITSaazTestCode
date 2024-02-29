using Contracts.DTOs;
using Contracts.DTOs.Person;

namespace Services.Interfaces
{
    public interface IPersonServices
    {
        Task<IEnumerable<FetchPersonDto>?> GetAllPeopleAsync();
        Task<FetchPersonDto?> GetPersonByIdAsync(int id);
        Task<ResultDto<FetchPersonDto>> CreateAsync(CreatePersonDto dto);
        Task<ResultDto<FetchPersonDto>> UpdateAsync(UpdatePersonDto dto);
        Task<ResultDto> DeleteAsync(int id);
    }
}
