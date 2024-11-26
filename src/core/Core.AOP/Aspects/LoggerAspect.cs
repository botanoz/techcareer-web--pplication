using Castle.DynamicProxy;
using Core.AOP.Helpers;
using Core.AOP.Interceptors;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Serilog;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;


namespace Core.AOP.Aspects
{
    public class LoggerAspect : MethodInterception
    {
        private LoggerServiceBase _loggerServiceBase;
        private IHttpContextAccessor _httpContextAccessor;

        protected override void OnBefore(IInvocation invocation)
        {

            var serviceProvider = ServiceTool.ServiceProvider;
            _loggerServiceBase ??= serviceProvider.GetService<LoggerServiceBase>();
            _httpContextAccessor ??= serviceProvider.GetService<IHttpContextAccessor>();

            var logParameters = new List<LogParameter>();
            for (var i = 0; i < invocation.Arguments.Length; i++)
            {
                logParameters.Add(new LogParameter
                {
                    Type = invocation.Arguments[i]?.GetType().Name ?? "null",
                    Value = invocation.Arguments[i]
                });
            }

            var logDetail = new LogDetail
            {
                MethodName = invocation.Method.Name,
                Parameters = logParameters,
                User = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "Unknown"
            };


            _loggerServiceBase.Info(JsonConvert.SerializeObject(logDetail));
        }
    }
}
