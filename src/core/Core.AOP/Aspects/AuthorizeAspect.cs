using System.Security.Authentication;
using Castle.DynamicProxy;
using Core.AOP.Helpers;
using Core.AOP.Interceptors;
using Core.Security.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Core.AOP.Aspects;

public class AuthorizeAspect : MethodInterception
{
    private string[] _roles;
    private IHttpContextAccessor _httpContextAccessor;

    public AuthorizeAspect(string roles)
    {
        _roles = roles.Split(',');
        _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
    }
    protected override void OnBefore(IInvocation invocation)
    {
        var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
        foreach (var role in _roles)
        {
            if (roleClaims.Contains(role))
            {
                return;
            }
        }
        throw new AuthenticationException("Authorization Denied");
    }
}