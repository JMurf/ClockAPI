using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace TimeClock.Controllers
{
    [RoutePrefix("api/{controller}/{id}")]
    public class ReportController : ApiController
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReportController()
        {
            log4net.Config.XmlConfigurator.Configure(
                    new FileInfo(HostingEnvironment.MapPath("~/log4net.config")));
        }
        [HttpPost]
        public IHttpActionResult Post(int id)
        {
            return Ok();
        }
    }
}