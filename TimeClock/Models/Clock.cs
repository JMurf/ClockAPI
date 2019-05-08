using System;
using System.Collections.Generic;

namespace TimeClock.Models
{
    public class Clock
    {
        public Clock()
        {
        }
        public string DeviceKey { get; set; }
        public bool OnLine { get; set; }
        public DateTime LastHeartbeat { get; set; }
        public List<TaskRoot> ActiveTasks { get; set; }
        public Queue<TaskRoot> Tasks { get; set; } 
    }
}