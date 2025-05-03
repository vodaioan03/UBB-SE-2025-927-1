using System;
using System.Threading.Tasks;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing paranthesis should not be followed by a space

namespace Duo.Services
{
    /// <summary>
    /// Service responsible for managing coin-related operations for users.
    /// </summary>
    public class CoinsService(CoinsServiceProxy serviceProxy) : ICoinsService
    {
        private readonly CoinsServiceProxy serviceProxy = serviceProxy;

        /// <summary>
        /// Gets the coin balance for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose coin balance is being queried.</param>
        /// <returns>The coin balance of the user, or 0 if an error occurs.</returns>
        public async Task<int> GetCoinBalanceAsync(int userId)
        {
            try
            {
                return await serviceProxy.GetUserCoinBalanceAsync(userId);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., to a logging service)
                Console.Error.WriteLine($"Error in GetCoinBalanceAsync: {ex.Message}");
                return 0; // Return a default value
            }
        }

        /// <summary>
        /// Tries to deduct coins from a user's wallet, based on a given cost.
        /// </summary>
        /// <param name="userId">The ID of the user trying to spend coins.</param>
        /// <param name="cost">The amount of coins to deduct.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public async Task<bool> TrySpendingCoinsAsync(int userId, int cost)
        {
            try
            {
                return await serviceProxy.TrySpendingCoinsAsync(userId, cost);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Error in TrySpendingCoinsAsync: {ex.Message}");
                return false; // Indicate failure
            }
        }

        /// <summary>
        /// Adds a specified amount of coins to the user's wallet.
        /// </summary>
        /// <param name="userId">The ID of the user receiving the coins.</param>
        /// <param name="amount">The amount of coins to add.</param>
        public async Task AddCoinsAsync(int userId, int amount)
        {
            try
            {
                await serviceProxy.AddCoinsAsync(userId, amount);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Error in AddCoinsAsync: {ex.Message}");
            }
        }

        /// <summary>
        /// Applies a daily login bonus to the user's wallet if they haven't already logged in today.
        /// </summary>
        /// <param name="userId">The ID of the user receiving the bonus. Defaults to 0.</param>
        /// <returns>True if the bonus was applied, false if the user has already logged in today.</returns>
        public async Task<bool> ApplyDailyLoginBonusAsync(int userId = 0)
        {
            try
            {
                return await serviceProxy.ApplyDailyLoginBonusAsync(userId);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.Error.WriteLine($"Error in ApplyDailyLoginBonusAsync: {ex.Message}");
                return false; // Indicate failure
            }
        }
    }
}
