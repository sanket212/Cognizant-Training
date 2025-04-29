using BankManagemetUsingOops;

namespace BankManagementUsingOops
{
    public class Program
    {
        public static void Main()
        {
            BankAccount bankAccount = new BankAccount();
            bool isExited =false;

            while(!isExited)
            {
                Console.WriteLine("\nBank Management System");
                Console.WriteLine("1.Create Bank Account");
                Console.WriteLine("2.Deposit money");
                Console.WriteLine("3.Withdraw money");
                Console.WriteLine("4.Show Account details");
                Console.WriteLine("5. Exit");
                Console.WriteLine("\nChoose a option");
                int choice = int.Parse(Console.ReadLine());


                switch(choice)
                {
                    case 1:
                        Console.WriteLine("Enter your account number");
                        int accountNumber = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter your Name");
                        string accountHolderName = Console.ReadLine();
                        Console.WriteLine("Enter your initial Balance");
                        decimal initialBalance= decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Enter type of account \t Savings or Current");
                        string type = Console.ReadLine();

                        bankAccount.CreateAccount(accountNumber, accountHolderName, initialBalance,type);
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
                        break;
                    default:
                        Console.WriteLine("Invalid option , please try again!");
                        break;
                }
            }
        }
    }
}