using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagemetUsingOops
{
    public interface IBankAccount
    {
        void CreateAccount(int accountNumebr, string accountName, decimal initalBalance, string accountType);
        void Deposit();
        void Withdraw();
        void DisplayDetials();
    }
}
