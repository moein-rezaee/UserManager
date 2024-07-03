using CustomResponse.Models;
using Fetch;
using Mapster;
using UserManager.Common.CustomResponse;
using UserManager.DTOs;
using UserManager.Enums;


namespace UserManager.Services
{
    public class VerifyService(FetchHttpRequest fetch)
    {
        private FetchHttpRequest _fetch { get; init; } = fetch;
        private AuthenticateService _authenticateService = new(fetch);

        private async Task<Result> GetFinalResult(Result result)
        {
            Result? resFrom = await _fetch.GetData<Result>(result.Data);
            return resFrom ?? result;
        }

        private async Task<Result> SmsVerifyCode(SmsVerifyCodeDto dto)
        {
            string Url = $@"/api/otp/v1/sms/verify-code";
            Result result = await _fetch.Post(Url, dto);
            return await GetFinalResult(result);
        }

        private async Task<Result> EmailVerifyCode(EmailVerifyCodeDto dto)
        {
            string Url = $@"/api/otp/v1/email/verify-code";
            Result result = await _fetch.Post(Url, dto);
            return await GetFinalResult(result);
        }

        private async Task<Result> SmsCode(SendCodeDto dto)
        {
            string Url = $@"/api/otp/v1/sms/send-code";
            Result result = await _fetch.Post(Url, dto);
            return await GetFinalResult(result);
        }

        private async Task<Result> EmailCode(EmailCodeDto dto)
        {
            string Url = $@"/api/otp/v1/email/send-code";
            Result result = await _fetch.Post(Url, dto);
            return await GetFinalResult(result);
        }

        public async Task<Result> Verify(VerifyDto dto)
        {
            Result result = new();

            switch (dto.Type)
            {
                case VerifyType.Phone:
                    result = await SmsVerifyCode(dto.Adapt<SmsVerifyCodeDto>());

                    break;

                case VerifyType.Email:
                    result = await EmailVerifyCode(dto.Adapt<EmailVerifyCodeDto>());
                    break;
            }


            if (result.IsOK())
            {
                result = await _authenticateService.AddUser(dto.Adapt<RegisterDto>());
                if (result.Status || result.IsCodeOK())
                {
                    //TODO: Add SessionLog
                    result = await _authenticateService.GenerateToken(dto.Adapt<GenerateTokenDto>());
                }
            }
            return result;
        }

        public async Task<Result> Sms(SendDto dto)
        {
            Result result = await _authenticateService.Organization(dto.OrganizationId);
            if (!result.IsOK())
                return result;

            return await SmsCode(dto.Adapt<SendCodeDto>());
        }

        public async Task<Result> Email(SendDto dto)
        {
            Result result = await _authenticateService.Organization(dto.OrganizationId);
            if (!result.IsOK())
                return result;

            return await EmailCode(dto.Adapt<EmailCodeDto>());
        }
    }
}
