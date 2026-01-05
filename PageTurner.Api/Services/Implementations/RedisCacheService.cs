// using System;
// using System.Text.Json;
// using System.Threading.Tasks;
// using PageTurner.Api.Infrastructure.Redis;
// using PageTurner.Api.Services.Interfaces;
// using StackExchange.Redis;

// namespace PageTurner.Api.Services.Implementations
// {
//     // Commenting out RedisCacheService implementation
//     // public class RedisCacheService : ICacheService
//     // {
//     //     private readonly IDatabase _database;

//     //     public RedisCacheService(RedisConnection redisConnection)
//     //     {
//     //         _database = redisConnection.GetDatabase();
//     //     }

//     //     public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
//     //     {
//     //         var json = JsonSerializer.Serialize(value);
//     //         await _database.StringSetAsync(key, json, expiration);
//     //     }

//     //     public async Task<T?> GetAsync<T>(string key)
//     //     {
//     //         var value = await _database.StringGetAsync(key);

//     //         if (!value.HasValue)
//     //             return default;

//     //         return JsonSerializer.Deserialize<T>(value!);
//     //     }

//     //     public async Task RemoveAsync(string key)
//     //     {
//     //         await _database.KeyDeleteAsync(key);
//     //     }
//     // }
// }
