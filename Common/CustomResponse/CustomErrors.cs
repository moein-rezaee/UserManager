using CustomResponse.Models;

namespace CustomResponse
{
    public class CustomErrors
    {
        public static Result InvalidInputData(object? data = null) => new()
        {
            Message = new()
            {
                Fa = "پارامترهای ورودی معتبر نمی باشد",
                En = "Invalid Input Data"
            },
            Data = data,
            StatusCode = StatusCodes.Status400BadRequest,
            Status = false
        };


        public static Result HttpRequestFailed(HttpResponseMessage data) => new()
        {
            Message = new Message()
            {
                Fa = "خطا هنگام ارسال درخواست",
                En = "Send Http Request Failed!"
            },
            Data = data,
            StatusCode = (int)data.StatusCode,
            Status = false
        };

        public static Result SendCodeFailed(object? data = null) => new()
        {
            Message = new Message()
            {
                Fa = "خطا هنگام ارسال کد",
                En = "Send Code Failed!"
            },
            Data = data,
            StatusCode = StatusCodes.Status500InternalServerError,
            Status = false
        };

        public static Result VerifyUserFailed(object? data = null) => new()
        {
            Message = new Message()
            {
                Fa = "خطا هنگام احراز هویت کاربر",
                En = "Verify User Failed!"
            },
            Data = data,
            StatusCode = StatusCodes.Status500InternalServerError,
            Status = false
        };
    }
}
