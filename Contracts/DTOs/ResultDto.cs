
using Contracts.Enums;

namespace Contracts.DTOs
{
    public class ResultDto
    {
        public StatusEnum Status { get; set; } = StatusEnum.Success;
        public string? Message { get; set; }
    }

    public class ResultDto<T> where T : class
    {
        public T? Result { get; set; }

        public StatusEnum Status { get; set; } = StatusEnum.Success;
        public string? Message { get; set; }
    }
}
