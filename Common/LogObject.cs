using System;
using CustomResponse.Models;
using NuGet.Protocol;

namespace UserManager.Common
{
    public class LogObject
    {

        public Guid TrackId { get; set; } = Guid.NewGuid();
        public string ServiceName { get; set; } = "UserManager";
        public string Message { get; set; } = "";
        public object? Data { get; set; }
        public DateTime At { get; set; } = DateTime.UtcNow;
        public string Type { get; set; } = "Error";

        public static string Info(Result result)
        {
            return new LogObject()
            {
                Message = result.Message.En,
                Data = result
            }.ToJson();
        }

        public static string Error(Exception e)
        {
            return new LogObject()
            {
                Message = $" [Message]: {e.Message} [InnerException] => {e.InnerException}",
                Data = e,
                Type = "Information"
            }.ToJson();
        }
    }
}

