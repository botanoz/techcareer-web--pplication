using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Castle.DynamicProxy;
using Core.AOP.Helpers;
using Core.AOP.Interceptors;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.AOP.Aspects;

public class CacheAspect : MethodInterception
{
     private readonly string _cacheKey;
    private readonly bool _bypassCache;
    private readonly string? _cacheGroupKey;
    private readonly IDistributedCache _cache;
    private readonly CacheSettings _cacheSettings;
    private readonly IConfiguration _configuration;
    private readonly string _cacheKeyTemplate;

    public CacheAspect(
        string cacheKeyTemplate,
        bool bypassCache,
        string? cacheGroupKey
        )
    {
        _cacheKeyTemplate = cacheKeyTemplate ?? throw new ArgumentNullException(nameof(cacheKeyTemplate));
        _bypassCache = bypassCache;
        _cacheGroupKey = cacheGroupKey;
     

        _cache = ServiceTool.ServiceProvider.GetService<IDistributedCache>();
        _configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
        
        
        _cache = _cache ?? throw new ArgumentNullException(nameof(_cache));
        _cacheSettings = _configuration?.GetSection("CacheSettings").Get<CacheSettings>() 
                         ?? throw new InvalidOperationException("CacheSettings could not be resolved.");
    }

    protected override void OnBefore(IInvocation invocation)
    {
        if (_bypassCache)
        {
            invocation.Proceed();
            return;
        }

     
        string cacheKey = GenerateCacheKey(invocation);


        var cachedResponse = _cache.GetAsync(cacheKey).Result;
        if (cachedResponse != null)
        {
         
            if (invocation.Method.ReturnType.IsGenericType &&
                invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var returnType = invocation.Method.ReturnType.GetGenericArguments()[0];
                var cachedValue = JsonSerializer.Deserialize(Encoding.UTF8.GetString(cachedResponse), returnType);

         
                var taskResultType = typeof(Task).GetMethod(nameof(Task.FromResult))!.MakeGenericMethod(returnType);
                invocation.ReturnValue = taskResultType.Invoke(null, new[] { cachedValue });
            }
            else
            {
                var cachedValue = JsonSerializer.Deserialize(Encoding.UTF8.GetString(cachedResponse), invocation.Method.ReturnType);
                invocation.ReturnValue = cachedValue;
            }
            return;
        }

    
        invocation.Proceed();
        if (invocation.Method.ReturnType == typeof(Task))
        {
            invocation.ReturnValue = InterceptAsync((Task)invocation.ReturnValue, cacheKey);
        }
        else if (invocation.Method.ReturnType.IsGenericType &&
                 invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
   
            var returnType = invocation.Method.ReturnType.GetGenericArguments()[0];
            var method = typeof(CacheAspect).GetMethod(nameof(InterceptAsyncGeneric), BindingFlags.NonPublic | BindingFlags.Instance);
            var genericMethod = method!.MakeGenericMethod(returnType);
            invocation.ReturnValue = genericMethod.Invoke(this, new[] { invocation.ReturnValue, cacheKey });
        }
        else
        {
            AddToCache(cacheKey, invocation.ReturnValue);
        }
    }


    private async Task InterceptAsync(Task task, string cacheKey)
    {
        await task.ConfigureAwait(false);
        AddToCache(cacheKey, null);
    }


    private async Task<T> InterceptAsyncGeneric<T>(Task<T> task, string cacheKey)
    {
        var result = await task.ConfigureAwait(false);
        AddToCache(cacheKey, result);
        return result;
    }

    private string GenerateCacheKey(IInvocation invocation)
    {
        var methodParameters = invocation.Method.GetParameters();
        var parameterDictionary = new Dictionary<string, object>();

        for (int i = 0; i < methodParameters.Length; i++)
        {
            parameterDictionary[methodParameters[i].Name!] = invocation.Arguments[i];
        }
        
        string cacheKey = _cacheKeyTemplate;

        foreach (var (key, value) in parameterDictionary)
        {
            cacheKey = cacheKey.Replace($"{{{key}}}", value?.ToString() ?? string.Empty);
        }

        return cacheKey;
    }

    private void AddToCache(string cacheKey, object response)
    {
        var slidingExpiration = TimeSpan.FromMinutes(_cacheSettings.SlidingExpiration);

        var cacheOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = slidingExpiration
        };

        var serializedResponse = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
        _cache.SetAsync(cacheKey, serializedResponse, cacheOptions).Wait();

   
        if (!string.IsNullOrEmpty(_cacheGroupKey))
        {
            AddCacheKeyToGroup(_cacheGroupKey, cacheKey, slidingExpiration).Wait();
        }
    }

    private async Task AddCacheKeyToGroup(string cacheGroupKey, string cacheKey, TimeSpan slidingExpiration)
    {
        var cacheGroup = await _cache.GetAsync(cacheGroupKey);
        HashSet<string> cacheKeysInGroup;

        if (cacheGroup != null)
        {
       
            cacheKeysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.UTF8.GetString(cacheGroup))!;
            if (!cacheKeysInGroup.Contains(cacheKey))
                cacheKeysInGroup.Add(cacheKey);
        }
        else
        {
       
            cacheKeysInGroup = new HashSet<string> { cacheKey };
        }

        var serializedGroup = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cacheKeysInGroup));

        var groupOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = slidingExpiration
        };

       
        await _cache.SetAsync(cacheGroupKey, serializedGroup, groupOptions);
    }
    
    
}