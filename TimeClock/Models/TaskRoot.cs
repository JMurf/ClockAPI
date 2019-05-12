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

        public string pass { get; set; }
        public int taskNo { get; set; }
        public bool result { get; set; }
        public string interfaceName { get; set; }
        [JsonIgnore]
        public DataProc processData { get; set; }

        public ClockTask()
        {
            taskNo = (new Random()).Next(Int32.MaxValue);
        }
        public override string ToString()
        {
            return "Task No: " + taskNo + "   InterfaceName: " + interfaceName;
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
            return (this.taskNo.Equals(other.taskNo));
        }
        public override int GetHashCode()
        {
            return taskNo;
        }
    }
}
