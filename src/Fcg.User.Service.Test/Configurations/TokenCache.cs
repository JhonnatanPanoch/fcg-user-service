using Fcg.User.Service.Application.Interfaces;
using Fcg.User.Service.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Fcg.User.Service.Test.Configurations;

public static class TokenCache
{
    private static string _adminToken;
    private static string _userToken;

    public static string GetAdminToken(IServiceProvider services)
    {
        if (!string.IsNullOrEmpty(_adminToken))
            return _adminToken;

        var jwtService = services.GetRequiredService<IJwtAppService>();

        var token = jwtService.GerarToken(Guid.NewGuid(), "admin@email.com", RoleEnum.Admin.ToString());
        _adminToken = token;
       
        return token;
    }

    public static string GetUserToken(IServiceProvider services)
    {
        if (!string.IsNullOrEmpty(_userToken))
            return _userToken;

        var jwtService = services.GetRequiredService<IJwtAppService>();

        var token = jwtService.GerarToken(Guid.NewGuid(), "user@email.com", RoleEnum.Usuario.ToString());
        _userToken = token;
        
        return token;
    }
}
