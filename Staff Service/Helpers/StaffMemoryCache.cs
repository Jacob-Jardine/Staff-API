using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Staff_Service.DomainModel;
using Staff_Service.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Staff_Service.Helpers
{
    public class StaffMemoryCache : IStaffMemoryCache
    {
        private readonly IStaffRepository _staffRepository;
        public readonly IMemoryCache _memoryCache;

        public StaffMemoryCache(IStaffRepository staffRepository, IMemoryCache memoryCache)
        {
            _staffRepository = staffRepository;
            _memoryCache = memoryCache;
        }

        public void AutomateCache()
        {
            RegisterCache("GetAllStaff", null, EvictionReason.None, null);
        }

        private MemoryCacheEntryOptions GetMemoryCacheEntryOptions()
        {
            int cacheExpirationMinutes = 1;
            DateTime cacheExpirationTime = DateTime.Now.AddMinutes(cacheExpirationMinutes);
            CancellationChangeToken cacheExpirationToken = new CancellationChangeToken
            (
                new CancellationTokenSource(TimeSpan.FromMinutes(cacheExpirationMinutes + 0.01)).Token
            );

            return new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(cacheExpirationTime)
                .SetPriority(CacheItemPriority.NeverRemove)
                .AddExpirationToken(cacheExpirationToken)
                .RegisterPostEvictionCallback(callback: RegisterCache, state: this);
        }

        private async void RegisterCache(object key, object value, EvictionReason reason, object state)
        {
            IEnumerable<StaffDomainModel> staffDomainModels = await _staffRepository.GetAllStaffAsync();
            _memoryCache.Set(key, staffDomainModels, GetMemoryCacheEntryOptions());
        }
    }
}
