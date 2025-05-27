using System.ComponentModel.DataAnnotations;

namespace BankManagementMvcApp.Models
{
    public class DepositWithdrawViewModel
    {
        [Required(ErrorMessage = "Account Number is required.")]
        [Display(Name = "Account Number")]
        [Range(1000000000, 2147483647, ErrorMessage = "Account Number must be a 10-digit number.")]
        public int AccountNumber { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Amount must be a positive number.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
