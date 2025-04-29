using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagementUsingLoops
{
    public class BankAccount
    {
        public int AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public decimal BankBalance { get; set; }

        public BankAccount(int accountNumber, string accountHolderName, decimal initialBalance) { 
            AccountNumber = accountNumber;
            accountHolderName = AccountHolderName;
            BankBalance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            BankBalance += amount;
            Console.WriteLine($"Deposited: {amount}, New Balance:{BankBalance}");
        }

        public void Withdraw(decimal amount) { 
            if(BankBalance >= amount)
            {
                BankBalance -= amount;
                Console.WriteLine($"Withdrawn amount: {amount}, New Balance: {BankBalance}");
            }
            else
            {
                Console.WriteLine("Insufficient balance in your account");
            }
        }

        public void DisplayDetails()
        {
            Console.WriteLine($"AccountNumber: {AccountNumber}, Name: {AccountHolderName},Balance: Rs.{BankBalance}");
        }
    }
}
