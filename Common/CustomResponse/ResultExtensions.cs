using CustomResponse.Models;

namespace UserManager.Common.CustomResponse
{
    public static class ResultExtensions
    {
        public static bool IsCodeOK(this Result result) => result.Code is not null && result.Code == StatusCodes.Status200OK;
        public static bool IsOK(this Result result) => result.StatusCode == StatusCodes.Status200OK || result.StatusCode == StatusCodes.Status202Accepted;
    }
}