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
        public Request ActiveReq { get; set; }
        public List<Request> Reqs { get; set; }

        public void DoTask(Request req)
        {
            Reqs.Add(req);
            req.Status = RequestStatus.QUEUED;
            //System.Threading.Thread.Sleep(10000);
            //req.mre.Set();
        }
        public bool HasQueuedRequest()
        {
            for (int i = 0; i < Reqs.Count; i++ )
            {
                if( Reqs[i].Status == RequestStatus.QUEUED)
                {
                    return true;
                }
            }
            return false;
        }

    }

}