using System;
using System.Collections.Generic;
using System.Text;

namespace sampleAccount.Models
{
    public enum OperationStatus
    {
        Ok = 0,
        Failed = 1,
        NotEnoughMoney = 200,
        AccountNotFound = 400,
    }
}
