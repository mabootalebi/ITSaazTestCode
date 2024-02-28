
namespace Contracts.DTOs
{
    public class ResultDto
    {
        public bool HasError { get; set; } = false;
        public string? Message { get; set; }
    }

    public class ResultDto<T> where T : class
    {
        public T? Result { get; set; }

        public bool HasError { get; set; } = false;
        public string? Message { get; set; }
    }
}
