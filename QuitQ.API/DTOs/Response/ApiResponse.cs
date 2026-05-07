namespace QuitQ.API.DTOs.Response
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public static ApiResponse<T> Success(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                Status = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Fail(List<string> errors, string message = "Failed")
        {
            return new ApiResponse<T>
            {
                Status = false,
                Message = message,
                Errors = errors
            };
        }
    }
}