using System.ComponentModel.DataAnnotations;

namespace BankManagementMvcApp.Models
{
    public class Account
    {
        [Required]
        [Display(Name = "Account Number")]
        [Range(1000000000, 2147483647, ErrorMessage = "Account Number must be a 10-digit number.")] // Enforce 10 digits
        public int AccountNumber { get; set; }

        [Required(ErrorMessage = "Account Holder Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        [Display(Name = "Account Holder Name")]
        public string AccountHolderName { get; set; }

        [Required(ErrorMessage = "Bank Balance is required.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Balance must be a positive number.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Bank Balance")]
        public decimal BankBalance { get; set; }

        [Required(ErrorMessage = "Account Type is required.")]
        [Display(Name = "Account Type")]
        public AccountType Type { get; set; }
    }
}
