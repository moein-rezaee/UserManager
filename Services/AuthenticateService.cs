using CustomResponse.Models;
using Fetch;
using Mapster;
using UserManager.DTOs;

namespace UserManager.Services
{
    public class AuthenticateService(FetchHttpRequest fetch)
    {
        private FetchHttpRequest _fetch { get; init; } = fetch;

        private async Task<Result> GetFinalResult(Result result)
        {
            Result? resFrom = await _fetch.GetData<Result>(result.Data);
            return resFrom ?? result;
        }

        internal async Task<Result> Organization(Guid id)
        {
            string Url = $@"/api/authenticate/v1/organization/{id}";
            Result result = await _fetch.Get(Url);
            return await GetFinalResult(result);
        }
        internal async Task<Result> AddUser(RegisterDto dto)
        {
            string Url = $@"/api/authenticate/v1/user/add";
            Result result = await _fetch.Post(Url, dto.Adapt<AddUserDto>());
            return await GetFinalResult(result);
        }
        internal async Task<Result> GenerateToken(GenerateTokenDto dto)
        {
            FetchRequestOptions options = new()
            {
                Url = $@"/api/authenticate/v1/user/generate-token",
                Data = dto
            };
            Result result = await _fetch.Post(options);
            return await GetFinalResult(result);
        }
    }
}