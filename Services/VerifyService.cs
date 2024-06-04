using CustomResponce.Models;
using Fetch;
using UserManager.DTOs;


namespace UserManager.Services
{
    public class VerifyService(FetchHttpRequest fetch)
    {
        private FetchHttpRequest _fetch { get; init; } = fetch;

        private async Task<Result> VerifyCode(string Code)
        {
            string Url = $@"/api/otp/v1/verify-code/{Code}";
            return await _fetch.Get(Url);
        }

        private async Task<Result> CreateUser(Guid OrganizationId, string Username)
        {
            FetchRequestOptions options = new()
            {
                Url = $@"/api/authenticate/v1/user/create-user",
                Data = new GenerateTokenDto()
                {
                    OrganizationId = OrganizationId,
                    Username = Username,
                    Password = Username
                }
            };
            return await _fetch.Get(options);
        }

        private async Task<Result> GenerateToken(Guid OrganizationId, string Username)
        {
            FetchRequestOptions options = new()
            {
                Url = $@"/api/authenticate/v1/user/generate-token",
                Data = new GenerateTokenDto()
                {
                    OrganizationId = OrganizationId,
                    Username = Username,
                    Password = Username
                }
            };
            return await _fetch.Post(options);
        }

        public bool UserIsExist(int? Code) => Code is not null && Code == 200;
        public bool CodeIsValid(int StatusCode) => StatusCode == 202;

        public async Task<Result> Verify(VerifyDto dto)
        {
            _ = new Result();
            Result result = await VerifyCode(dto.Code);
            if (CodeIsValid(result.StatusCode))
            {
                result = await CreateUser(dto.OrganizationId, dto.Username);
                if(result.Status || UserIsExist(result.Code)) {
                    result = await GenerateToken(dto.OrganizationId, dto.Username);
                }
            }
            return result;
        }
    }
}
