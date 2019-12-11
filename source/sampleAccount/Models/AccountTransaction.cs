using System;
using System.Collections.Generic;
using System.Text;

namespace sampleAccount.Models
{
    public class AccountTransaction
    {
        public string AccountName { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
    }
}
