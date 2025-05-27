namespace BankManagementMvcApp.Models.Services
{
    public interface IBankAccount
    {
        public interface IBankAccountService
        {
            Task<int?> CreateAccountAsync(string accountName, decimal initialBalance, string accountType);
            Task<bool> DepositAsync(int accountNumber, decimal amount);
            Task<bool> WithdrawAsync(int accountNumber, decimal amount);
            Task<Account> GetAccountDetailsAsync(int accountNumber);
            Task<decimal?> GetCurrentBalanceAsync(int accountNumber); // Helper method
        }
    }
}
