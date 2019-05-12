using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using TimeClock.DTOs;

namespace TimeClock.Models
{
    public class ClockTask
    {
        public delegate List<RecordDTO> DataProc(PunchClockDTO tr);

        public string Pass { get; set; }
        public int TaskNo { get; set; }
        public bool Result { get; set; }
        public string InterfaceName { get; set; }
        [JsonIgnore]
        public DataProc ProcessData { get; set; }

        public ClockTask()
        {
            TaskNo = (new Random()).Next(Int32.MaxValue);
        }
        public override string ToString()
        {
            return "Task No: " + TaskNo + "   InterfaceName: " + InterfaceName;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            ClockTask objAsTask = obj as ClockTask;
            if (objAsTask == null) return false;
            else return Equals(objAsTask);
        }
        public bool Equals(ClockTask other)
        {
            if (other == null) return false;
            return (this.TaskNo.Equals(other.TaskNo));
        }
        public override int GetHashCode()
        {
            return TaskNo;
        }
    }
}
