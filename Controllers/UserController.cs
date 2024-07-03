using CustomResponse;
using CustomResponse.Models;
using Fetch;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserManager.Common;
using FluentValidation.Results;
using UserManager.DTOs;
using UserManager.Services;
using UserManager.Enums;
using Mapster;
using UserManager.Common.CustomResponse;

namespace UserManager.Controllers
{
    [ApiController]
    [Route("[action]")]

    public class UserController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly ILogger<UserController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IValidator<VerifyDto> _verifyValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly AuthenticateService _authenticateService;
        private readonly VerifyService _verifyService;

        public UserController(
            IConfiguration config,
            ILogger<UserController> logger,
            IValidator<VerifyDto> verifyValidator,
            IValidator<LoginDto> loginValidator,
            IValidator<RegisterDto> registerValidator,
            IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _verifyValidator = verifyValidator;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;

            string? baseUrl = _config.GetValue<string>("BaseUrl");
            FetchHttpRequest fetch = FetchHttpRequest.GetInstance(_httpClientFactory, baseUrl);

            _authenticateService = new AuthenticateService(fetch);
            _verifyService = new VerifyService(fetch);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            Result result = new();
            try
            {
                ValidationResult check = _registerValidator.Validate(dto);
                if (!check.IsValid)
                {
                    result = CustomErrors.InvalidInputData(check.Errors.Adapt<List<ValidationError>>());
                    return StatusCode(result.StatusCode, result);
                }

                result = await _authenticateService.AddUser(dto);


                _logger.LogInformation(LogObject.Info(result));
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception e)
            {
                _logger.LogInformation(LogObject.Error(e));
                result = CustomErrors.SendCodeFailed();
                return StatusCode(result.StatusCode, result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            Result result = new();
            try
            {
                // Validation
                ValidationResult check = _loginValidator.Validate(dto);
                if (!check.IsValid)
                {
                    result = CustomErrors.InvalidInputData(check.Errors.Adapt<List<ValidationError>>());
                    return StatusCode(result.StatusCode, result);
                }

                switch (dto.Type)
                {
                    case LoginType.Password:
                        result = await _authenticateService.GenerateToken(dto.Adapt<GenerateTokenDto>());
                        break;
                    case LoginType.Phone:
                        result = await _verifyService.Sms(dto.Adapt<SendDto>());
                        break;
                    case LoginType.Email:
                        result = await _verifyService.Email(dto.Adapt<SendDto>());
                        break;
                }

                _logger.LogInformation(LogObject.Info(result));
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception e)
            {
                _logger.LogInformation(LogObject.Error(e));
                result = CustomErrors.SendCodeFailed();
                return StatusCode(result.StatusCode, result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Verify(VerifyDto dto)
        {
            Result result = new();
            try
            {
                ValidationResult check = _verifyValidator.Validate(dto);
                if (!check.IsValid)
                {
                    result = CustomErrors.InvalidInputData();
                    return StatusCode(result.StatusCode, result);
                }

                result = await _verifyService.Verify(dto);

                _logger.LogInformation(LogObject.Info(result));
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception e)
            {
                _logger.LogInformation(LogObject.Error(e));
                result = CustomErrors.VerifyUserFailed();
                return StatusCode(result.StatusCode, result);
            }
        }
    }
}