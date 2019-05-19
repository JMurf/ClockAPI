using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using TimeClock.DTOs;
using TimeClock.Models;

namespace PunchClock.Controllers
{
    [RoutePrefix("api/{controller}/{id}")]
    public class PunchController : ApiController
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        delegate void HandleReqDelegate(Request ct);
        //delegate void TimeoutDelegate(Clock clock, RequestFindRecords req);

        public PunchController()
        {
            log4net.Config.XmlConfigurator.Configure(
                    new FileInfo(HostingEnvironment.MapPath("~/log4net.config")));
        }
        [HttpPost]
        public IHttpActionResult Post(PunchRequestDTO pReq)
        {
            PunchResponseDTO pResp = new PunchResponseDTO();
            /* retrieve clock based on DeviceKey */
            if (Global.ClockList.Count == 0 || 
                    Global.ClockList.ContainsKey(pReq.DeviceKey) == false)  /* testing */
            {
                Clock c = new Clock();
                c.DeviceKey = pReq.DeviceKey;
                c.OnLine = true;
                c.LastHeartbeat = DateTime.Now;
                c.Reqs = new List<Request>();
                c.ActiveReqs = new List<Request>();
                Global.ClockList.Add(c.DeviceKey, c);
            }
            Clock clock = Global.ClockList[pReq.DeviceKey];
            if (clock != null )
            {
                /* create a new clock req - hardcoded for now */
                RequestFindRecords req = new RequestFindRecords();
                req.fill();

                req.mre = new ManualResetEvent(false);
                HandleReqDelegate reqDelegate = new HandleReqDelegate(clock.DoTask);

                /* requested interface */
                reqDelegate.Invoke(req);
                req.mre.WaitOne();

                pResp.Status = 0;
                pResp.Msg = "Clock Found";
                pResp.PunchData = req.PunchList;
                return Ok(pResp);
            }
            else
            {
                pResp.Msg = "Clock not found";
                pResp.DeviceKey = pReq.DeviceKey;
                pResp.Status = -1;
                pResp.PunchData = null;
                return Ok(pResp);
            }
        }
        private void ReqTimeout(Clock clock, RequestFindRecords req )
        {
            Thread.Sleep(120000);
            for (int i = 0; i < clock.Reqs.Count; i++)
            {
                if (clock.Reqs[i].TaskNo == req.TaskNo)
                {
                    clock.Reqs.RemoveAt(i);
                    break;
                }
            }
            for (int i = 0; i < clock.ActiveReqs.Count; i++)
            {
                if (clock.ActiveReqs[i].TaskNo == req.TaskNo)
                {
                    clock.ActiveReqs.RemoveAt(i);
                    break;
                }
            }
            req.mre.Set();
        }
        [HttpPost]
        public IHttpActionResult Post(int id)
        {
            log.Info("This is a post request with id field: " + id);
            return Ok();
        }
    }
}