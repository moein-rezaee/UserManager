using CustomResponse;
using CustomResponse.Models;
using Fetch;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserManager.Common;
using FluentValidation.Results;
using UserManager.DTOs;
using UserManager.Services;

namespace MyApp.Namespace
{
    [ApiController]
    [Route("[action]")]

    public class UserController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly ILogger<UserController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IValidator<VerifyDto> _verifyValidator;
        private readonly IValidator<SendDto> _sendValidator;
        private readonly VerifyService _service;

        public UserController(
            IConfiguration config,
            ILogger<UserController> logger,
            IValidator<VerifyDto> verifyValidator,
            IValidator<SendDto> sendValidator,
            IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _verifyValidator = verifyValidator;
            _sendValidator = sendValidator;

            string? baseUrl = _config.GetSection("BaseGatewayUrl").Value;
            FetchHttpRequest fetch = FetchHttpRequest.GetInstance(_httpClientFactory, baseUrl);
            _service = new VerifyService(fetch);
        }

        [HttpPost]
        public async Task<IActionResult> Send(SendDto dto)
        {
            Result result = new();
            try
            {
                // Validation
                ValidationResult check = _sendValidator.Validate(dto);
                if (!check.IsValid)
                {
                    result = CustomErrors.InvalidInputData();
                    return StatusCode(result.StatusCode, result);
                }

                result = await _service.Send(dto);

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

                result = await _service.Verify(dto);

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