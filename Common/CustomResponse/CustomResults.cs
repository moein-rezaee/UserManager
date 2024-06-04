
using CustomResponce.Models;

namespace CustomResponce
{
    public class CustomResults
    {

        public static Result HttpRequestOk(HttpResponseMessage data) => new()
        {
            Message = new Message()
            {
                Fa = "عملیات با موفقیت به پایان رسید",
                En = "Good Job. Result is Ok!"
            },
            Data = data,
            StatusCode = (int)data.StatusCode,
        };
    }
}