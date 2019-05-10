using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeClock.DTOs
{
    public class PunchRequestDTO
    {
        public string DeviceKey { get; set; }   /* which clock */
        public string StartTime { get; set; }   /* punches >= to this datetime */
        public string EndTime { get; set; }     /* punches <= to this datetime */
        public int PersonId { get; set; }       /* employee id, -1 equals all employees */
    }
}