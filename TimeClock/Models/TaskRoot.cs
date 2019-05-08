using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeClock.DTOs;

namespace TimeClock.Models
{
    public class TaskRoot
    {
        public delegate void DataProc(PunchClockDTO tr);

        public string pass { get; set; }
        public int taskNo { get; set; }
        public bool result { get; set; }
        public string interfaceName { get; set; }
        public DataProc processData { get; set; }

        public TaskRoot()
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
            TaskRoot objAsTask = obj as TaskRoot;
            if (objAsTask == null) return false;
            else return Equals(objAsTask);
        }
        public bool Equals(TaskRoot other)
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