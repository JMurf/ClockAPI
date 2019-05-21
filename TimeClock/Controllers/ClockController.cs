﻿using System;
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
        public IHttpActionResult Post(int id, ClockDTO postData)
        {
            string idStr = "Heartbeat: \n";
            if( id == 2 ) idStr = "Retrieve Request: \n";
            if (id == 3) idStr = "Task Completion: \n";
            log.Info(idStr + JsonConvert.SerializeObject(postData));
            ReturnParam rp = new ReturnParam();
            if (postData == null || postData.DeviceKey == null)
            {
                rp.Result = false;
                rp.Msg = "No post postData or missing attributes";
                return Ok(rp);
            }
            Clock clock = null;    /* clock making post request */
            switch (id)
            {
                case 1:  /* periodic 'heartbeat' of the punchclocks */
                    /* update our clock collection */
                    if ( Global.ClockList.ContainsKey(postData.DeviceKey) )
                    {
                        clock = Global.ClockList[postData.DeviceKey];
                        clock.OnLine = true;
                        clock.LastHeartbeat = DateTime.Now;
                    }
                    else /* new clock checking in, add to list */
                    {
                        try
                        {
                            clock = new Clock
                            {
                                DeviceKey = postData.DeviceKey,
                                LastHeartbeat = DateTime.Now,
                                Reqs = new List<Request>(),
                                ActiveReq = null,
                                OnLine = true
                            };
                        }
                        catch (Exception e)
                        {
                            log.Info("--------Exception occurred-------\n" + e);
                        }
                        if( clock != null )
                        {
                            Global.ClockList.Add(postData.DeviceKey, clock);
                        }
                    }
                    /* if there are any tasks for the clock, set result flag to true */
                    rp.Result = clock.HasQueuedRequest();
                    return Ok(rp);

                case 2: /* clock retrieves req */
                    if (Global.ClockList.ContainsKey(postData.DeviceKey))
                    {
                        clock = Global.ClockList[postData.DeviceKey];
                        Request req = null;
                        for( int i=0; i<clock.Reqs.Count; i++ )
                        {
                            if (clock.Reqs[i].Status == RequestStatus.QUEUED)
                            {
                                req = clock.Reqs[i];
                                log.Info("New Request Info: " + JsonConvert.SerializeObject(req));
                                if (req.InterfaceName.Equals("findRecords"))
                                {
                                    log.Info("Task #" + req.TaskNo + " added to active tasks.");
                                    req.Status = RequestStatus.ACTIVE;
                                    req.Result = "true";
                                    return Ok((RequestFindRecords)req);
                                }
                                else
                                {
                                    log.Info("Removing task #" + req.TaskNo + ": unknown interface.");
                                    clock.Reqs.Remove(req);
                                }
                            }
                        }
                    }
                    rp.Result = false; /* if no req or can't find clock */
                    return Ok(rp);
                case 3: /* result of clock executing req */
                    rp.Result = false;
                    if (Global.ClockList.ContainsKey(postData.DeviceKey))
                    {
                        clock = Global.ClockList[postData.DeviceKey];
                        log.Info("Task completion");
                        log.Info(JsonConvert.SerializeObject(postData));

                        Request req = null;
                        for (int i = 0; i < clock.Reqs.Count; i++)
                        {
                            if (clock.Reqs[i].TaskNo.Equals(postData.TaskNo))
                            {
                                req = clock.Reqs[i];
                                break;
                            }
                        }
                        if (req != null)
                        {
                            req.ProcessData(postData);
                            log.Info("Marking request complete: " + req.TaskNo);
                            req.Status = RequestStatus.COMPLETED;
                            req.mre.Set();
                        }
                        rp.Result = clock.HasQueuedRequest();
                    }
                    return Ok(rp);
                default:
                    rp.Msg = "Invalid Index: " + id;
                    log.Info(JsonConvert.SerializeObject(rp));
                    return Ok(false);
            }
        }
    }
}
