using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        delegate void HandleTaskDelegate(ClockTask ct);

        public PunchController()
        {
            log4net.Config.XmlConfigurator.Configure(
                    new FileInfo(HostingEnvironment.MapPath("~/log4net.config")));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(PunchRequestDTO pReq)
        {
            PunchResponseDTO pResp = new PunchResponseDTO();
            /* retrieve clock based on DeviceKey */
            Clock clock = Global.ClockList[pReq.DeviceKey];
            if (clock != null )
            {
                /* create a new task */
                TaskFindRecords task = new TaskFindRecords();
                task.InterfaceName = "findRecords";
                task.Pass = "123456";
                task.startTime = pReq.StartTime;
                task.endTime = pReq.EndTime;

                HandleTaskDelegate taskDelegate = new HandleTaskDelegate(clock.DoTask);
                IAsyncResult ar = taskDelegate.BeginInvoke(null, null, task);

                taskDelegate.EndInvoke(ar);
                pResp.Status = 0;
                pResp.Msg = "Clock Found";
                pResp.Punches = task.getData();
                return Ok(pResp);
            }
            else
            {
                pResp.Msg = "Clock not found";
                pResp.DeviceKey = pReq.DeviceKey;
                pResp.Status = -1;
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