using sampleAccount.Models;

namespace sampleAccount.Web.Models
{
    public class TransactionModel
    {
        public decimal Amount { get; set; }
        public string TargetAccountNumber { get; set; }
        public TransactionType Type { get; set; }
    }
}