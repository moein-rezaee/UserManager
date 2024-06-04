using System.Reflection;
using Mapster;
using UserManager.DTOs;
namespace UserManager.Mappings;

public static class MapsterConfig
{
	public static void RegisterMapsterConfiguration(this IServiceCollection services)
	{
		TypeAdapterConfig<SendDto, SendCodeDto>.NewConfig().Map(dto => dto.Mobile, s => s.Username);
		TypeAdapterConfig<VerifyDto, VerifyCodeDto>.NewConfig().Map(dto => dto.Mobile, s => s.Username);

		TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
	}
}
