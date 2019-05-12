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
        public List<ClockTask> ActiveTasks { get; set; }
        public Queue<ClockTask> Tasks { get; set; }

        public void DoTask(ClockTask task)
        {
            //Tasks.Enqueue(task);
            System.Threading.Thread.Sleep(20000);
        }
    }

}