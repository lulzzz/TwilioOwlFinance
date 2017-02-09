namespace Twilio.OwlFinance.Domain.Model
{
    public class ApiResponse<T> : IGenericResponse<T>
        where T : class
    {
        public ApiResponse()
        { }

        public ApiResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public string Message { get; set; }
        public bool HasError { get; set; }
        public int StatusCode { get; set; }
    }
}
