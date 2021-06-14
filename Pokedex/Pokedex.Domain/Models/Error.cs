namespace Pokedex.Domain.Models
{

    public class ApiErrorResponse
    {
        public ApiErrorResponse()
        {
            this.Error = new Error();
        }
        public Error Error { get; set; }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
