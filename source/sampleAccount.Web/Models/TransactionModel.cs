using sampleAccount.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sampleAccount.Web.Models
{
    public class TransactionModel
    {
        public decimal Amount { get; set; }
        public string TargetAccountNumber { get; set; }
        public TransactionType Type { get; set; }
    }
}
