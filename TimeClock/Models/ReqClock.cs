using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using TimeClock.DTOs;

namespace TimeClock.Models
{
    public enum RequestStatus
    {
        COMPLETED, INITIATED, QUEUED, ACTIVE, CANT_COMPLETE
    }
    public class Request
    {
        public delegate void DataProc(ClockDTO tr);
        [JsonIgnore]
        public RequestStatus Status { get; set; }
        public string Pass { get; set; }
        public int TaskNo { get; set; }     /* unique task (req) identifier */
        public string Result { get; set; }
        public string InterfaceName { get; set; }
        public ClockDTO Data { get; set; }
        [JsonIgnore]
        public ManualResetEvent mre { get; set; }

        [JsonIgnore]
        public DataProc ProcessData { get; set; }

        public Request()
        {
            TaskNo = (new Random()).Next(Int32.MaxValue);
            Status = RequestStatus.INITIATED;
        }
        public override string ToString()
        {
            return "Task No: " + TaskNo + "   InterfaceName: " + InterfaceName;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Request objAsReq = obj as Request;
            if (objAsReq == null) return false;
            else return Equals(objAsReq);
        }
        public bool Equals(Request other)
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
