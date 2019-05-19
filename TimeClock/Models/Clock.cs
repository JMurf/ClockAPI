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
        public List<Request> ActiveReqs { get; set; }
        public List<Request> Reqs { get; set; }

        public void DoTask(Request req)
        {
                Reqs.Add(req);
            //System.Threading.Thread.Sleep(10000);
            //req.mre.Set();
        }
    }

}