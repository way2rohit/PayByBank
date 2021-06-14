namespace Pokedex.Domain.Models
{
    public class BaseResponse<T>
    {
        public bool IsSuccess
        {
            get
            {
                if (string.IsNullOrEmpty(ErrorMessage)) //&& ErrorCode > 0
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public T Data { get; set; }

    }
}
