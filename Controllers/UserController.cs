using System.Net.Http.Headers;
using CustomResponce;
using CustomResponce.Models;
using Fetch;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using UserManager.Common;
using UserManager.DTOs;
using UserManager.Services;
using UserManager.ViewModels;

namespace MyApp.Namespace
{
    [ApiController]
    [Route("[action]")]

    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IValidator<VerifyDto> _verifyValidator;
        private readonly VerifyService _service;

        public UserController(
            ILogger<UserController> logger,
            IValidator<VerifyDto> verifyValidator,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _verifyValidator = verifyValidator;

            string baseUrl = "http://localhost:5227";
            FetchHttpRequest fetch = FetchHttpRequest.GetInstance(_httpClientFactory, baseUrl);
            _service = new VerifyService(fetch);
        }


        [HttpPost]
        public async Task<IActionResult> Verify(VerifyDto dto)
        {
            var result = new Result();
            try
            {
                // Validation
                var check = _verifyValidator.Validate(dto);
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