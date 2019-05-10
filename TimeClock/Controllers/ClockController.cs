using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimeClock.DTOs;
using TimeClock.Models;
using System.IO;
using System.Web.Hosting;
using System.Web.Helpers;
using Newtonsoft.Json;

namespace TimeClock.Controllers
{
    class ReturnParam
    {
        public bool Result { get; set; }
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
            log.Info(data);
            string temp = JsonConvert.SerializeObject(data);
            log.Info(temp);
            ReturnParam rp = new ReturnParam();
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
                    else
                    {
                        try
                        {
                            clock = new Clock
                            {
                                DeviceKey = data.DeviceKey,
                                LastHeartbeat = DateTime.Now,
                                Tasks = new Queue<TaskRoot>(),
                                ActiveTasks = new List<TaskRoot>(),
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
                    //should we add a task?
                    /* !!TEMP!! lets hardcode a FindRecords punches task */
                    if(clock.Tasks.Count == 0 && new Random().NextDouble() > .2)
                    {
                        TaskFindRecords task = new TaskFindRecords();
                        task.fill();
                        log.Info("Task #" + task.taskNo + " added.");
                        clock.Tasks.Enqueue(task);
                    }

                    /* if there are any tasks for the clock, set result flag to true */
                    rp.Result = clock.Tasks.Count > 0;
                    return Ok(rp);

                case 2: /* clock retrieves task */
                    if (Global.ClockList.ContainsKey(data.DeviceKey))
                    {
                        clock = Global.ClockList[data.DeviceKey];
                        if( clock.Tasks.Count > 0 )
                        {
                            TaskRoot task = clock.Tasks.Dequeue();
                            if( task.interfaceName.Equals("findRecords"))
                            {
                                clock.ActiveTasks.Add(task); /* tasks in progress */
                                log.Info("Task #" + task.taskNo + " added to active tasks.\nSize: " + 
                                            clock.ActiveTasks.Count);
                                return Ok((TaskFindRecords)task);
                            }
                        }
                    }
                    rp.Result = false; /* if no task or can't find clock */
                    return Ok(rp);
                case 3: /* result of clock executing task */
                    if (Global.ClockList.ContainsKey(data.DeviceKey))
                    {
                        clock = Global.ClockList[data.DeviceKey];
                        log.Info("Task completion");
                        log.Info("Removing: " + data.TaskNo);
                        log.Info(data);
                        TaskRoot task = clock.ActiveTasks.Find(x => x.taskNo == data.TaskNo);
                        if( task != null )
                        {
                            clock.ActiveTasks.Remove(task);
                            log.Info("Found task\n------------------");
                            task.processData(data);
                        }
                        else
                        {
                            log.Info("Task not found in active list.\n---------------------");
                        }
                        return Ok(clock.Tasks.Count > 0);
                    }
                    rp.Result = false;
                    return Ok(rp);
                default:
                    return Ok(false);
            }
        }
    }
}
