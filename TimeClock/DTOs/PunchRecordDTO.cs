using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeClock.DTOs
{
    public class PunchRecordDTO
    {
        public string DeviceKey { get; set; }   /* clock ID */
        public string Time { get; set; }        /* time of punch */

    }
}