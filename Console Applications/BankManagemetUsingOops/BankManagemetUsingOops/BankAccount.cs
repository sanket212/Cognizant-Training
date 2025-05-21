using System;
using System.Data;
using Microsoft.Data.SqlClient;
using BankManagemetUsingOops.Models;

namespace BankManagemetUsingOops
{
    public class BankAccount : IBankAccount
    {
        private readonly string _connectionString;
        private static readonly Random _random = new Random();

        public BankAccount(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");
            }
            _connectionString = connectionString;
        }

        private int GenerateUniqueAccountNumber()
        {
            int newAccountNumber;
            bool isUnique = false;
            do
            {
                newAccountNumber = _random.Next(1000000000, 2000000000);

                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        string sql = "SELECT COUNT(*) FROM Accounts WHERE AccountNumber = @AccountNumber";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@AccountNumber", newAccountNumber);
                            int count = (int)command.ExecuteScalar();
                            if (count == 0)
                            {
                                isUnique = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking account number uniqueness: {ex.Message}");
                    isUnique = false;
                }
            } while (!isUnique);

            return newAccountNumber;
        }

        public void CreateAccount(int accountNumebr, string accountName, decimal initialBalance, string accountType)
        {
            int newAccountNumber = GenerateUniqueAccountNumber();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("CreateAccount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", newAccountNumber);
                        command.Parameters.AddWithValue("@AccountHolderName", accountName);
                        command.Parameters.AddWithValue("@BankBalance", initialBalance);
                        command.Parameters.AddWithValue("@AccountType", accountType);

                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine($"Account created successfully with Account Number: {newAccountNumber}.");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    Console.WriteLine("Error: A rare duplicate account number was generated. Please try creating account again.");
                }
                else
                {
                    Console.WriteLine($"Database error creating account: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while creating account: {ex.Message}");
            }
        }

        public void Deposit()
        {
            Console.Write("Enter your account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number. Please enter a valid number.");
                return;
            }

            Console.Write("Enter the amount to be deposited: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal depositAmount) || depositAmount <= 0)
            {
                Console.WriteLine("Invalid deposit amount. Please enter a positive number.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    decimal currentBalance = 0;
                    using (SqlCommand command = new SqlCommand("GetAccountBalance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        object result = command.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            Console.WriteLine("Account not found, please try again!");
                            return;
                        }
                        currentBalance = (decimal)result;
                    }

                    decimal newBalance = currentBalance + depositAmount;
                    using (SqlCommand command = new SqlCommand("UpdateAccountBalance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        command.Parameters.AddWithValue("@NewBalance", newBalance);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine($"Deposited Amount: {depositAmount}, New Balance: {GetCurrentBalance(accountNumber)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during deposit: {ex.Message}");
            }
        }

        public void Withdraw()
        {
            Console.Write("Enter your account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number. Please enter a valid number.");
                return;
            }

            Console.Write("Enter the amount to be withdrawn: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount) || withdrawAmount <= 0)
            {
                Console.WriteLine("Invalid withdrawal amount. Please enter a positive number.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    decimal currentBalance = 0;
                    using (SqlCommand command = new SqlCommand("GetAccountBalance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        object result = command.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            Console.WriteLine("Account not found, please try again!");
                            return;
                        }
                        currentBalance = (decimal)result;
                    }

                    if (currentBalance < withdrawAmount)
                    {
                        Console.WriteLine("Insufficient balance for withdrawal.");
                        return;
                    }

                    decimal newBalance = currentBalance - withdrawAmount;
                    using (SqlCommand command = new SqlCommand("UpdateAccountBalance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        command.Parameters.AddWithValue("@NewBalance", newBalance);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine($"Withdrawn Amount: {withdrawAmount}, New Balance: {GetCurrentBalance(accountNumber)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during withdrawal: {ex.Message}");
            }
        }

        public void DisplayDetials()
        {
            Console.Write("Enter your account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number. Please enter a valid number.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("GetAccountDetails", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int accNum = reader.GetInt32(reader.GetOrdinal("AccountNumber"));
                                string accHolderName = reader.GetString(reader.GetOrdinal("AccountHolderName"));
                                decimal balance = reader.GetDecimal(reader.GetOrdinal("BankBalance"));
                                string accType = reader.GetString(reader.GetOrdinal("AccountType"));

                                Console.WriteLine($"Account Number: {accNum}, Name: {accHolderName}, Balance: {balance}, Account Type: {accType}");
                            }
                            else
                            {
                                Console.WriteLine("Account not found, please try again!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying details: {ex.Message}");
            }
        }

        private decimal GetCurrentBalance(int accountNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("GetAccountBalance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        object result = command.ExecuteScalar();
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
            return 0;
        }
    }
}