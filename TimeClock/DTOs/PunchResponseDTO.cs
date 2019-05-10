using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeClock.DTOs
{
    public class PunchResponseDTO
    {
        public string DeviceKey { get; set; }
        public string Msg { get; set; }
        public int Status { get; set; }
        public List<PunchRecordDTO> Punches { get; set; }
    }
}