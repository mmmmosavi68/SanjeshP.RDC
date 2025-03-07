using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SanjeshP.RDC.Data;
using SanjeshP.RDC.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SanjeshP.RDC.WebFramework.Middlewares
{
    public class AccessService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ApplicationDbContext _dbContext;

        public AccessService(IDistributedCache distributedCache, ApplicationDbContext dbContext)
        {
            _distributedCache = distributedCache;
            _dbContext = dbContext;
        }

        public async Task<List<Menu>> GetUserAccessMenusAsync(Guid userId)
        {
            var cacheKey = $"UserAccessMenus_{userId}";
            var cachedData = await _distributedCache.GetStringAsync(cacheKey);

            if (string.IsNullOrEmpty(cachedData))
            {
                // داده‌ها در کش موجود نیستند
                //var menus = _dbContext.UserAccessMenus
                //    .Where(uam => uam.UserId == userId)
                //    .Select(uam => uam.Menu)
                //    .ToList();

                // تبدیل داده‌ها به JSON برای ذخیره در Redis
                //cachedData = JsonConvert.SerializeObject(menus);
                //await _distributedCache.SetStringAsync(cacheKey, cachedData, new DistributedCacheEntryOptions
                //{
                //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                //});

                //return menus;
            }

            // تبدیل داده‌های JSON به لیست
            return JsonConvert.DeserializeObject<List<Menu>>(cachedData);
        }
    }

}
