using sampleAccount.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sampleAccount.Web
{
    public class SettingConfiguration : ISettingConfiguration
    {
        public decimal DepositFeeInPercent { get; set; }
    }
}
