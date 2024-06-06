using CustomResponse.Models;
using Fetch;
using Mapster;
using UserManager.DTOs;


namespace UserManager.Services
{
    public class VerifyService(FetchHttpRequest fetch)
    {
        private FetchHttpRequest _fetch { get; init; } = fetch;

        private async Task<Result> GetFinalResult(Result result)
        {
            Result? resFrom = await _fetch.GetData<Result>(result.Data);
            return resFrom ?? result;
        }

        private async Task<Result> VerifyCode(VerifyCodeDto dto)
        {
            string Url = $@"/api/otp/v1/verify-code";
            Result result = await _fetch.Post(Url, dto);
            return await GetFinalResult(result);
        }

        private async Task<Result> SendCode(SendCodeDto dto)
        {
            string Url = $@"/api/otp/v1/send-code";
            Result result = await _fetch.Post(Url, dto);
            return await GetFinalResult(result);
        }

        private async Task<Result> Organization(Guid id)
        {
            string Url = $@"/api/authenticate/v1/organization/{id}";
            Result result = await _fetch.Get(Url);
            return await GetFinalResult(result);
        }

        private async Task<Result> CreateUser(Guid OrganizationId, string Username)
        {
            string Url = $@"/api/authenticate/v1/user/create-user";
            GenerateTokenDto Data = new()
            {
                OrganizationId = OrganizationId,
                Username = Username,
                Password = Username
            };
            Result result = await _fetch.Post(Url, Data);
            return await GetFinalResult(result);

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
            Result result = await _fetch.Post(options);
            return await GetFinalResult(result);
        }

        public bool UserIsExist(int? Code) => Code is not null && Code == 200;
        public bool CodeIsValid(int StatusCode) => StatusCode == 202;
        public bool OrganizationIsValid(int StatusCode) => StatusCode == 200;

        public async Task<Result> Verify(VerifyDto dto)
        {
            _ = new Result();
            Result result = await VerifyCode(dto.Adapt<VerifyCodeDto>());
            if (CodeIsValid(result.StatusCode))
            {
                result = await CreateUser(dto.OrganizationId, dto.Username);
                if (result.Status || UserIsExist(result.Code))
                {
                    result = await GenerateToken(dto.OrganizationId, dto.Username);
                }
            }
            return result;
        }

        public async Task<Result> Send(SendDto dto)
        {
            Result result = await Organization(dto.OrganizationId);
            if (!OrganizationIsValid(result.StatusCode))
                return result;

            return await SendCode(dto.Adapt<SendCodeDto>());
        }
    }
}
