using System.Text;
using System.Text.Json;
using Castle.DynamicProxy;
using Core.AOP.Helpers;
using Core.AOP.Interceptors;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Core.AOP.Aspects;

public class ClearCacheAspect : MethodInterception
{
    private readonly string _cacheGroupKey;
    private readonly IDistributedCache _cache;

    public ClearCacheAspect(string cacheGroupKey)
    {
        _cacheGroupKey = cacheGroupKey ?? throw new ArgumentNullException(nameof(cacheGroupKey));
        _cache = ServiceTool.ServiceProvider.GetService<IDistributedCache>()
                 ?? throw new ArgumentNullException(nameof(_cache));
    }

    protected override void OnBefore(IInvocation invocation)
    {
        ClearCacheGroup(_cacheGroupKey).Wait();
        invocation.Proceed();
    }

    private async Task ClearCacheGroup(string cacheGroupKey)
    {
      
        var cacheGroup = await _cache.GetAsync(cacheGroupKey);
        if (cacheGroup != null)
        {
           
            var cacheKeys = JsonSerializer.Deserialize<HashSet<string>>(Encoding.UTF8.GetString(cacheGroup));
            if (cacheKeys != null)
            {
        
                foreach (var key in cacheKeys)
                {
                    await _cache.RemoveAsync(key);
                }
            }

      
            await _cache.RemoveAsync(cacheGroupKey);
        }
    }
}