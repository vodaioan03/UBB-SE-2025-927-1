using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Duo.Services
{
    public class CoinsServiceProxy
    {
        private readonly HttpClient httpClient;

        public CoinsServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<int> GetUserCoinBalanceAsync(int userId)
        {
            var response = await httpClient.GetFromJsonAsync<int>($"api/coins/balance/{userId}");
            return response;
        }

        public async Task<bool> TrySpendingCoinsAsync(int userId, int cost)
        {
            var response = await httpClient.PostAsJsonAsync("api/coins/spend", new { UserId = userId, Cost = cost });
            return response.IsSuccessStatusCode;
        }

        public async Task AddCoinsAsync(int userId, int amount)
        {
            await httpClient.PostAsJsonAsync("api/coins/add", new { UserId = userId, Amount = amount });
        }

        public async Task<bool> ApplyDailyLoginBonusAsync(int userId)
        {
            var response = await httpClient.PostAsJsonAsync("api/coins/dailybonus", new { UserId = userId });
            return response.IsSuccessStatusCode;
        }
    }
}
