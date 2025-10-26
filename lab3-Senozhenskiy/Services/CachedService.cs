using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Caching.Memory;
using SciencePortalWebApp.Data;

namespace SciencePortalWebApp.Services;

public class CachedService<T> : ICachedService<T> where T : class
{
    private readonly SciencePortalDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;

    public CachedService(SciencePortalDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public void AddEntities(string cacheKey, int rowsNumber = 20)
    {
        if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<T> entities))
        {
            entities = _dbContext.Set<T>().Take(rowsNumber).ToList();
            _memoryCache.Set(cacheKey, entities, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(272) // 2*16 + 240
            });
        }
    }

    public IEnumerable<T> GetEntities(string cacheKey, int rowsNumber = 20)
    {
        _memoryCache.TryGetValue(cacheKey, out IEnumerable<T> entities);
        return entities;
    }
}