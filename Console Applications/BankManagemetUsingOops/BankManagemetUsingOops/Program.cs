using BankManagemetUsingOops;
using Microsoft.Extensions.Configuration; // Add this using directive
using System;
using System.IO; // Add this using directive

namespace BankManagementUsingOops
{
    public class Program
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Main()
        {
            // 1. Build the configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Tells the builder where to look for appsettings.json
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Add your JSON file
                .Build();

            // 2. Pass the connection string to BankAccount
            // You'll need to modify the BankAccount constructor to accept the connection string.
            IBankAccount bankAccount = new BankAccount(Configuration.GetConnectionString("BankDBConnection"));

            bool isExited = false;

            while (!isExited)
            {
                Console.WriteLine("\n--- Bank Management System ---");
                Console.WriteLine("1. Create Bank Account");
                Console.WriteLine("2. Deposit Money");
                Console.WriteLine("3. Withdraw Money");
                Console.WriteLine("4. Show Account Details");
                Console.WriteLine("5. Exit");
                Console.Write("\nChoose an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter account holder name: ");
                        string accountHolderName = Console.ReadLine();
                        Console.Write("Enter initial balance: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal initialBalance) || initialBalance < 0)
                        {
                            Console.WriteLine("Invalid initial balance. Please enter a non-negative number.");
                            break;
                        }
                        Console.Write("Enter type of account (Savings or Current): ");
                        string type = Console.ReadLine();

                        if (!Enum.TryParse(typeof(AccountType), type, true, out _))
                        {
                            Console.WriteLine("Invalid account type. Please enter 'Savings' or 'Current'.");
                            break;
                        }

                        bankAccount.CreateAccount(0, accountHolderName, initialBalance, type);
                        break;

                    case 2:
                        bankAccount.Deposit();
                        break;
                    case 3:
                        bankAccount.Withdraw();
                        break;
                    case 4:
                        bankAccount.DisplayDetials();
                        break;
                    case 5:
                        isExited = true;
                        Console.WriteLine("Exiting Bank Management System. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again!");
                        break;
                }
            }
        }
    }
}