using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sampleAccount.DAL.Data
{
    public class AccountEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string OwenerId { get; set; }

        [Required]
        public string IBAN { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual IList<TransactionEntity> Transactions{ get; set; }
    }
}
