using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeClock.Models
{
    public static class Global
    {
        public static Dictionary<string, Clock> ClockList = new Dictionary<string, Clock>();
    }
}