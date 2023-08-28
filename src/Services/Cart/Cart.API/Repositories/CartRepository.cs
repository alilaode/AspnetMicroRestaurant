using Cart.API.Entities;
using Cart.API.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Cart.API.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IDistributedCache _redisCache;

        public CartRepository(IDistributedCache cache)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<Bill> GetCart(string userName)
        {
            var bill = await _redisCache.GetStringAsync(userName);

            if (String.IsNullOrEmpty(bill))
                return null;

            return JsonConvert.DeserializeObject<Bill>(bill);
        }

        public async Task<Bill> UpdateCart(Bill bill)
        {
            await _redisCache.SetStringAsync(bill.UserName, JsonConvert.SerializeObject(bill));

            return await GetCart(bill.UserName);
        }

        public async Task DeleteCart(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
    }
}
