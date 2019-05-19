using System;
using System.Collections.Generic;
using System.Web.Http;
using TimeClock.DTOs;
using TimeClock.Models;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace TimeClock.Controllers
{
    class ReturnParam
    {
        public bool Result { get; set; }
        public string Msg { get; set; }
    }
    [RoutePrefix("api/{controller}/{id}")]
    public class ClockController : ApiController   
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ClockController()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(HostingEnvironment.MapPath("~/log4net.config")));
        }

        [HttpPost]
        public IHttpActionResult Post(int id, PunchClockDTO data)
        {
            log.Info(JsonConvert.SerializeObject(data));
            ReturnParam rp = new ReturnParam();
            if (data == null || data.DeviceKey == null)
            {
                rp.Result = false;
                rp.Msg = "No post data or missing attributes";
                return Ok(rp);
            }
            Clock clock = null;    /* clock making post request */
            switch (id)
            {
                case 1:  /* periodic 'heartbeat' of the punchclocks */
                    /* update our clock collection */
                    if ( Global.ClockList.ContainsKey(data.DeviceKey) )
                    {
                        clock = Global.ClockList[data.DeviceKey];
                        clock.OnLine = true;
                        clock.LastHeartbeat = DateTime.Now;
                    }
                    else /* new clock checking in, add to list */
                    {
                        try
                        {
                            clock = new Clock
                            {
                                DeviceKey = data.DeviceKey,
                                LastHeartbeat = DateTime.Now,
                                Reqs = new List<Request>(),
                                ActiveReqs = new List<Request>(),
                                OnLine = true
                            };
                        }
                        catch (Exception e)
                        {
                            log.Info("--------Exception occurred-------\n" + e);
                        }
                        if( clock != null )
                        {
                            Global.ClockList.Add(data.DeviceKey, clock);
                        }
                    }
                    //should we add a req?
                    /* !!TEMP!! lets hardcode a FindRecords punches req */
                    if(clock.Reqs.Count == 0 && new Random().NextDouble() > .99999)
                    {
                        RequestFindRecords task = new RequestFindRecords();
                        task.fill();
                        log.Info("Task #" + task.TaskNo + " added.");
                        clock.Reqs.Add(task);
                    }

                    /* if there are any tasks for the clock, set result flag to true */
                    rp.Result = clock.Reqs.Count > 0;
                    return Ok(rp);

                case 2: /* clock retrieves req */
                    if (Global.ClockList.ContainsKey(data.DeviceKey))
                    {
                        clock = Global.ClockList[data.DeviceKey];
                        if( clock.Reqs.Count > 0 )
                        {
                            Request req = clock.Reqs[0];
                            clock.Reqs.RemoveAt(0);
                            if( req.InterfaceName.Equals("findRecords"))
                            {
                                clock.ActiveReqs.Add(req); /* tasks in progress */
                                log.Info("Task #" + req.TaskNo + " added to active tasks.\nSize: " + 
                                            clock.ActiveReqs.Count);
                                return Ok((RequestFindRecords)req);
                            }
                        }
                    }
                    rp.Result = false; /* if no req or can't find clock */
                    return Ok(rp);
                case 3: /* result of clock executing req */
                    if (Global.ClockList.ContainsKey(data.DeviceKey))
                    {
                        clock = Global.ClockList[data.DeviceKey];
                        log.Info("Task completion");
                        log.Info("Removing: " + data.TaskNo);
                        log.Info(data);
                        Request req = clock.ActiveReqs.Find(x => x.TaskNo == data.TaskNo);
                        if( req != null )
                        {
                            clock.ActiveReqs.Remove(req);
                            log.Info("Found req\n------------------");
                            req.ProcessData(data);
                            req.mre.Set();
                        }
                        else
                        {
                            log.Info("Task not found in active list.\n---------------------");
                        }
                        rp.Result = clock.Reqs.Count > 0;
                        return Ok(rp);
                    }
                    rp.Result = false;
                    return Ok(rp);
                default:
                    return Ok(false);
            }
        }
    }
}
