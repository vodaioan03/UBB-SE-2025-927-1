using System.Diagnostics.CodeAnalysis;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Controller for managing user coin balances and transactions.
    /// Provides endpoints for retrieving balances, spending coins, adding coins, and applying daily bonuses.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CoinsController"/> class with the specified repository.
    /// </remarks>
    /// <param name="repository">The repository instance to be used for data access.</param>
    [ApiController]
    [Route("api/coins")]
    [ExcludeFromCodeCoverage]
    public class CoinsController(IRepository repository) : BaseController(repository)
    {
        #region Methods

        /// <summary>
        /// Retrieves the coin balance for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The coin balance of the user.</returns>
        [HttpGet("balance/{userId}")]
        public async Task<ActionResult<int>> GetUserCoinBalance(int userId)
        {
            // Retrieve the user's coin balance from the repository
            var balance = await repository.GetUserCoinBalanceAsync(userId);
            return Ok(balance);
        }

        /// <summary>
        /// Deducts coins from a user's wallet for a specific transaction.
        /// </summary>
        /// <param name="request">The request containing the user ID and the cost to deduct.</param>
        /// <returns>An HTTP response indicating success or failure.</returns>
        [HttpPost("spend")]
        public async Task<ActionResult> SpendCoins([FromBody] SpendCoinsRequest request)
        {
            // Attempt to deduct coins from the user's wallet
            bool success = await repository.TryDeductCoinsFromUserWalletAsync(request.UserId, request.Cost);
            if (success)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Not enough coins.");
            }
        }

        /// <summary>
        /// Adds coins to a user's wallet.
        /// </summary>
        /// <param name="request">The request containing the user ID and the amount to add.</param>
        /// <returns>An HTTP response indicating success.</returns>
        [HttpPost("add")]
        public async Task<ActionResult> AddCoins([FromBody] AddCoinsRequest request)
        {
            // Add coins to the user's wallet
            await repository.AddCoinsToUserWalletAsync(request.UserId, request.Amount);
            return Ok();
        }

        /// <summary>
        /// Applies a daily login bonus to a user's wallet if they haven't already claimed it today.
        /// </summary>
        /// <param name="request">The request containing the user ID.</param>
        /// <returns>An HTTP response indicating success or failure.</returns>
        [HttpPost("dailybonus")]
        public async Task<ActionResult> ApplyDailyLoginBonus([FromBody] DailyBonusRequest request)
        {
            // Retrieve the user's last login time
            DateTime lastLogin = await repository.GetUserLastLoginTimeAsync(request.UserId);

            // Check if the bonus has already been claimed today
            if (lastLogin.Date < DateTime.Now.Date)
            {
                // Add the daily bonus to the user's wallet
                await repository.AddCoinsToUserWalletAsync(request.UserId, 10); // Example: 10 coins bonus

                // Update the user's last login time to now
                await repository.UpdateUserLastLoginTimeToNowAsync(request.UserId);
                return Ok();
            }
            else
            {
                return BadRequest("Bonus already claimed today.");
            }
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// Represents a request to spend coins from a user's wallet.
        /// </summary>
        public class SpendCoinsRequest
        {
            /// <summary>
            /// Gets or sets the unique identifier of the user.
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// Gets or sets the cost to deduct from the user's wallet.
            /// </summary>
            public int Cost { get; set; }
        }

        /// <summary>
        /// Represents a request to add coins to a user's wallet.
        /// </summary>
        public class AddCoinsRequest
        {
            /// <summary>
            /// Gets or sets the unique identifier of the user.
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// Gets or sets the amount of coins to add to the user's wallet.
            /// </summary>
            public int Amount { get; set; }
        }

        /// <summary>
        /// Represents a request to apply a daily login bonus to a user's wallet.
        /// </summary>
        public class DailyBonusRequest
        {
            /// <summary>
            /// Gets or sets the unique identifier of the user.
            /// </summary>
            public int UserId { get; set; }
        }

        #endregion
    }
}