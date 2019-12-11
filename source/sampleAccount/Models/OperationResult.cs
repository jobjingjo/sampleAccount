using System;
using System.Collections.Generic;
using System.Text;

namespace sampleAccount.Models
{
    public class OperationResult
    {
        public OperationStatus Status { get; set; }

        public decimal Balance { get; set; }
    }
}
