namespace AuthNest_API.DTOS
{
    public class ErrorResponseDto
    {
           
        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}

