using System.Reflection;
using Mapster;
using UserManager.DTOs;
namespace UserManager.Mappings;

public static class MapsterConfig
{
	public static void RegisterMapsterConfiguration(this IServiceCollection services)
	{
		TypeAdapterConfig<SendDto, SendCodeDto>.NewConfig().Map(dto => dto.Mobile, s => s.Username);
		TypeAdapterConfig<VerifyDto, SmsVerifyCodeDto>.NewConfig().Map(dto => dto.Mobile, s => s.Username);
		TypeAdapterConfig<VerifyDto, EmailVerifyCodeDto>.NewConfig().Map(dto => dto.Email, s => s.Username);

		TypeAdapterConfig<VerifyDto, GenerateTokenDto>.NewConfig().Map(dto => dto.Password, s => s.Username);

		TypeAdapterConfig<VerifyDto, RegisterDto>.NewConfig()
		.Map(dto => dto.Password, s => s.Username)
		.Map(dto => dto.RepPassword, s => s.Username)
		.Map(dto => dto.Email, s => s.Type == Enums.VerifyType.Email ? s.Username : "")
		.Map(dto => dto.Phone, s => s.Type == Enums.VerifyType.Phone ? s.Username : "");

		TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
	}
}
