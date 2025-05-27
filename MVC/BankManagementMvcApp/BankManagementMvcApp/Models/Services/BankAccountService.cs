using BankManagementMvcApp.Models;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using static BankManagementMvcApp.Models.Services.IBankAccount;

namespace BankManagementMvcApp.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly string _connectionString;
        private static readonly Random _random = new Random();

        public BankAccountService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // Helper to generate a unique 10-digit account number
        private async Task<int> GenerateUniqueAccountNumberAsync()
        {
            int newAccountNumber;
            bool isUnique = false;
            do
            {
                newAccountNumber = _random.Next(1000000000, 2000000000); // Ensures a 10-digit number

                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        await connection.OpenAsync();
                        string sql = "SELECT COUNT(*) FROM Accounts WHERE AccountNumber = @AccountNumber";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@AccountNumber", newAccountNumber);
                            int count = (int)await command.ExecuteScalarAsync();
                            if (count == 0)
                            {
                                isUnique = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // In a real application, you would use a logging framework here (e.g., Serilog, NLog)
                    Console.WriteLine($"Error checking account number uniqueness: {ex.Message}");
                    isUnique = false; // Assume not unique if an error occurs
                }
            } while (!isUnique);

            return newAccountNumber;
        }

        public async Task<int?> CreateAccountAsync(string accountName, decimal initialBalance, string accountType)
        {
            int newAccountNumber = await GenerateUniqueAccountNumberAsync();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("usp_CreateAccount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", newAccountNumber);
                        command.Parameters.AddWithValue("@AccountHolderName", accountName);
                        command.Parameters.AddWithValue("@BankBalance", initialBalance);
                        command.Parameters.AddWithValue("@AccountType", accountType);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return newAccountNumber; // Return the newly created account number
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error creating account: {ex.Message}");
                if (ex.Number == 2627) // Primary key violation
                {
                    // This should be rare with GenerateUniqueAccountNumberAsync, but good to handle
                    Console.WriteLine("A duplicate account number was encountered during creation.");
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while creating account: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DepositAsync(int accountNumber, decimal amount)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Get current balance using stored procedure
                    decimal? currentBalance = await GetCurrentBalanceAsync(accountNumber);
                    if (currentBalance == null)
                    {
                        return false; // Account not found
                    }

                    // Update balance using stored procedure
                    decimal newBalance = currentBalance.Value + amount;
                    using (SqlCommand command = new SqlCommand("usp_UpdateAccountBalance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        command.Parameters.AddWithValue("@NewBalance", newBalance);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during deposit: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> WithdrawAsync(int accountNumber, decimal amount)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Get current balance using stored procedure
                    decimal? currentBalance = await GetCurrentBalanceAsync(accountNumber);
                    if (currentBalance == null)
                    {
                        return false; // Account not found
                    }

                    if (currentBalance.Value < amount)
                    {
                        return false; // Insufficient balance
                    }

                    // Update balance using stored procedure
                    decimal newBalance = currentBalance.Value - amount;
                    using (SqlCommand command = new SqlCommand("usp_UpdateAccountBalance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        command.Parameters.AddWithValue("@NewBalance", newBalance);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during withdrawal: {ex.Message}");
                return false;
            }
        }

        public async Task<Account> GetAccountDetailsAsync(int accountNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("usp_GetAccountDetails", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Account
                                {
                                    AccountNumber = reader.GetInt32(reader.GetOrdinal("AccountNumber")),
                                    AccountHolderName = reader.GetString(reader.GetOrdinal("AccountHolderName")),
                                    BankBalance = reader.GetDecimal(reader.GetOrdinal("BankBalance")),
                                    Type = Enum.Parse<AccountType>(reader.GetString(reader.GetOrdinal("AccountType")))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying details: {ex.Message}");
            }
            return null; // Account not found or error occurred
        }

        public async Task<decimal?> GetCurrentBalanceAsync(int accountNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("usp_GetAccountBalance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        object result = await command.ExecuteScalarAsync();
                        if (result != DBNull.Value && result != null)
                        {
                            return (decimal)result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving balance: {ex.Message}");
            }
            return null;
        }
    }
}
