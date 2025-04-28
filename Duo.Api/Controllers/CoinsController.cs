using Microsoft.AspNetCore.Mvc;
using Duo.Api.Repositories;
using Duo.Api.Persistence;

namespace Duo.Api.Controllers
{
    [ApiController]
    [Route("api/coins")]
    public class CoinsController : BaseController
    {
        private readonly IRepository repository;

        public CoinsController(IRepository repository, DataContext dataContext) : base(dataContext)
        {
            this.repository = repository;
        }

        [HttpGet("balance/{userId}")]
        public async Task<ActionResult<int>> GetUserCoinBalance(int userId)
        {
            var balance = await repository.GetUserCoinBalanceAsync(userId);
            return Ok(balance);
        }

        [HttpPost("spend")]
        public async Task<ActionResult> SpendCoins([FromBody] SpendCoinsRequest request)
        {
            bool success = await repository.TryDeductCoinsFromUserWalletAsync(request.UserId, request.Cost);
            if (success)
                return Ok();
            else
                return BadRequest("Not enough coins.");
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCoins([FromBody] AddCoinsRequest request)
        {
            await repository.AddCoinsToUserWalletAsync(request.UserId, request.Amount);
            return Ok();
        }

        [HttpPost("dailybonus")]
        public async Task<ActionResult> ApplyDailyLoginBonus([FromBody] DailyBonusRequest request)
        {
            DateTime lastLogin = await repository.GetUserLastLoginTimeAsync(request.UserId);
            if (lastLogin.Date < DateTime.Now.Date)
            {
                await repository.AddCoinsToUserWalletAsync(request.UserId, 10); // Example: 10 coins bonus
                await repository.UpdateUserLastLoginTimeToNowAsync(request.UserId);
                return Ok();
            }
            else
            {
                return BadRequest("Bonus already claimed today.");
            }
        }
    }

    public class SpendCoinsRequest
    {
        public int UserId { get; set; }
        public int Cost { get; set; }
    }

    public class AddCoinsRequest
    {
        public int UserId { get; set; }
        public int Amount { get; set; }
    }

    public class DailyBonusRequest
    {
        public int UserId { get; set; }
    }
}
