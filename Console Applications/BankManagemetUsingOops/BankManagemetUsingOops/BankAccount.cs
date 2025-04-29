using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankManagemetUsingOops.Models;

namespace BankManagemetUsingOops
{
    public class BankAccount:IBankAccount
    {
        private readonly List<Account> _account;

        public BankAccount()
        {
            
            _account = new List<Account>();
        }

        public void CreateAccount(int accountNumebr, string accountName, decimal initalBalance, string accountType)
        {   
            var account = new Account()
            {
                AccountNumber = accountNumebr,
                AccountHolderName = accountName,
                BankBalance = initalBalance,
                Type = Enum.Parse<AccountType>(accountType)

            };
            _account.Add(account);
            Console.WriteLine("Account created successfully.");
        }

        public void Deposit()
        {
            Console.WriteLine("Enter your account number");
            int accountNumber = int.Parse(Console.ReadLine());
            var account = _account.Find(a => a.AccountNumber == accountNumber);

            if (account != null)
            {
                Console.WriteLine("Enter the amount to be deposited");
                decimal depositAmount = decimal.Parse(Console.ReadLine());
                account.BankBalance += depositAmount;
                Console.WriteLine($"Deposited Amount: {depositAmount},New Balance:{account.BankBalance}");

            }
            else
            {
                Console.WriteLine("Account not found, please try again!");
            }
        }

        public void DisplayDetials()
        {
            Console.WriteLine("Enter your account number");
            int accountNumber = int.Parse(Console.ReadLine());
            var account = _account.Find(a => a.AccountNumber == accountNumber);

            if (account != null)
            {
                Console.WriteLine($"Account Number: {account.AccountNumber}, Name: {account.AccountHolderName}, Baalance:{account.BankBalance}, Account Type: {account.Type}");

            }
            else
            {
                Console.WriteLine("Account not found, please try again!");
            }
        }

        public void Withdraw()
        {
            Console.WriteLine("Enter your account number");
            int accountNumber = int.Parse(Console.ReadLine());
            var account = _account.Find(a => a.AccountNumber == accountNumber);

            if (account != null)
            {
                Console.WriteLine("Enter the amount to be withdrawn");
                decimal withdrawAmount = decimal.Parse(Console.ReadLine());
                account.BankBalance -= withdrawAmount;
                Console.WriteLine($"Withdrawn Amount: {withdrawAmount},New Balance:{account.BankBalance}");

            }
            else
            {
                Console.WriteLine("Account not found, please try again!");
            }
        }
    }
}
