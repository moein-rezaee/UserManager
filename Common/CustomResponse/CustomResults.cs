
using CustomResponse.Models;

namespace CustomResponse
{
    public class CustomResults
    {

        public static Result HttpRequestOk(string data, int statusCode) => new()
        {
            Message = new Message()
            {
                Fa = "عملیات با موفقیت به پایان رسید",
                En = "Good Job. Result is Ok!"
            },
            Data = data,
            StatusCode = statusCode,
        };
    }
}