
namespace BankManagementUsingLoops
{
    public class Program
    {
        public static void Main()
        {
            List<BankAccount> accounts = new List<BankAccount>();
            bool isExit = false;

            while (!isExit)
            {
                Console.WriteLine("\nBank Management System");
                Console.WriteLine("1.Create Bank Account");
                Console.WriteLine("2.Deposit money");
                Console.WriteLine("3.Withdraw money");
                Console.WriteLine("4.Show Account details");
                Console.WriteLine("5. Exit");
                Console.WriteLine("\nChoose a option");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter your Account number");
                        int accountNumber = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter your name");
                        string accoutHolderName = Console.ReadLine();
                        Console.WriteLine("Enter your Initial Balance");
                        decimal initialBalance = decimal.Parse(Console.ReadLine());
                        accounts.Add(new BankAccount(accountNumber, accoutHolderName, initialBalance));
                        Console.WriteLine("\nAccount Created Successfully");
                        break;
                    case 2:
                        Console.WriteLine("Enter your Account Number");
                        accountNumber = int.Parse(Console.ReadLine());
                        BankAccount account = accounts.Find(a => a.AccountNumber == accountNumber);

                        if (account != null)
                        {
                            Console.WriteLine("Enter amount to be deposited");
                            decimal depositAmount = decimal.Parse(Console.ReadLine());
                            account.Deposit(depositAmount);

                            Console.WriteLine($"Amount Rs.{depositAmount} deposited successfully !");
                        }
                        else
                        {
                            Console.WriteLine("\nAccount not found");
                        }
                        break;
                    case 3:
                        Console.WriteLine("Enter your account number");
                        accountNumber = int.Parse(Console.ReadLine());
                        account = accounts.Find(a => a.AccountNumber == accountNumber);
                        if (account != null)
                        {
                            Console.WriteLine("Enter amount to be withdrawn");
                            decimal withdrawAmount = decimal.Parse(Console.ReadLine());
                            account.Withdraw(withdrawAmount);
                            Console.WriteLine($"\nAmount Rs.{withdrawAmount} withdrawn successfully!");
                        }
                        else
                        {
                            Console.WriteLine("\nAccount not found");
                        }
                        break;
                    case 4:
                        Console.WriteLine("Enter your account number");
                        accountNumber = int.Parse(Console.ReadLine());
                        account = accounts.Find(a => a.AccountNumber == accountNumber);

                        if (account != null)
                        {
                            account.DisplayDetails();
                        }
                        else
                        {
                            Console.WriteLine("\nAccount not found");
                        }
                        break;
                    case 5:
                        isExit = true;
                        break;
                    default:
                        Console.WriteLine("\nInvalid Option , please try again!");
                        break;
                }
            }



        }

    }
}

