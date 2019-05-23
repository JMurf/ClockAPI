using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

        delegate void HandleRequestDelegate(Request ct);

        public PunchController()
        {
            log4net.Config.XmlConfigurator.Configure(
                    new FileInfo(HostingEnvironment.MapPath("~/log4net.config")));
        }
        [HttpPost]
        public IHttpActionResult Post(PunchRequestDTO postData)
        {
            log.Info(JsonConvert.SerializeObject(postData));
            PunchResponseDTO pResp = new PunchResponseDTO();
            if( postData == null )
            {
                pResp.Msg = "Data is null";
                pResp.Status = RequestStatus.CANT_COMPLETE;
                return Ok(pResp);                
            }
            /* retrieve clock based on DeviceKey */
            if (Global.ClockList.Count == 0 || 
                    Global.ClockList.ContainsKey(postData.DeviceKey) == false)  /* testing */
            {
                Clock c = new Clock();
                c.DeviceKey = postData.DeviceKey;
                c.OnLine = true;
                c.LastHeartbeat = DateTime.Now;
                c.Reqs = new List<Request>();
                c.ActiveReq = null;
                Global.ClockList.Add(c.DeviceKey, c);
            }
            Clock clock = Global.ClockList[postData.DeviceKey];
            if (clock != null )
            {
                /* create a new clock req from postData */
                RequestFindRecords req = new RequestFindRecords();

                req.Status = RequestStatus.INITIATED;
                req.StartTime = postData.StartTime;
                req.EndTime = postData.EndTime;
                req.InterfaceName = "findRecords";
                req.PersonId = postData.PersonId;
                req.Length = postData.Length;
                req.Index = postData.Index;

                req.mre = new ManualResetEvent(false);
                HandleRequestDelegate reqDelegate = new HandleRequestDelegate(clock.DoTask);

                /* requested interface */
                reqDelegate.Invoke(req);
                req.mre.WaitOne(180 * 1000);  //3 minute timeout

                if( req.Status == RequestStatus.COMPLETED )
                {
                    pResp.Status = req.Status;
                    pResp.Msg = "Request " + req.TaskNo + " Completed";
                    pResp.DeviceKey = postData.DeviceKey;
                    pResp.Data = req.Data;
                }
                else
                {
                    pResp.Status = req.Status;
                    pResp.DeviceKey = postData.DeviceKey;
                    pResp.Msg = "Timeout before request completed";
                    pResp.Data = req.Data;
                    Request reqToDel = clock.Reqs.Find(x => x.TaskNo == req.TaskNo);
                    log.Info("Request " + reqToDel.TaskNo + " removed\n------------------");
                }
                return Ok(pResp);
            }
            else
            {
                pResp.Status = RequestStatus.CANT_COMPLETE;
                pResp.Msg = "Clock not found";
                pResp.DeviceKey = postData.DeviceKey;

                return Ok(pResp);
            }
        }
        [HttpPost]
        public IHttpActionResult Post(int id)
        {
            log.Info("This is a post request with id field: " + id);
            return Ok();
        }
    }
}