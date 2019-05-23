using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeClock.Models;

namespace TimeClock.DTOs
{
    public class PunchResponseDTO
    {
        public string DeviceKey { get; set; }
        public string Msg { get; set; }
        public RequestStatus Status { get; set; }
        public ClockDTO Data { get; set; }
    }
}