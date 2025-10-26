using System.Collections.Generic;

namespace SciencePortalWebApp.Services;

public interface ICachedService<T> where T : class
{
    void AddEntities(string cacheKey, int rowsNumber = 20);
    IEnumerable<T> GetEntities(string cacheKey, int rowsNumber = 20);
}