// Models/ErrorViewModel.cs
using System;

namespace BankManagementMvcApp.Models // Ensure this namespace matches your project's root namespace + .Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
