using System;
using System.Collections.Generic;
using System.Text;

namespace sampleAccount.Helpers
{
    public static class SystemDateTime
    {
        private static Func<DateTime> funcDateTime = () => DateTime.UtcNow;
        public static void Set(DateTime value)
        {
            funcDateTime = () => value;
        }
        public static void Reset()
        {
            funcDateTime = () => DateTime.UtcNow;
        }
        public static DateTime UtcNow()
        {
            return funcDateTime();
        }
    }
}
