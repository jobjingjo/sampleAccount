using sampleAccount.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sampleAccount.DAL.Data
{
    public class TransactionEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid FromId { get; set; }

        public virtual AccountEntity From { get; set; }

        public string AccountTo { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreateAt { get; set; }

        public TransactionType Type { get; set; }

        public OperationStatus Status { get; set; }
    }
}
