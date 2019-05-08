using System;
using System.IO;
using System.Web.Hosting;
using TimeClock.DTOs;

namespace TimeClock.Models
{
    public class TaskFindRecords : TaskRoot
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /* delegate */
        public void processFindRecords(PunchClockDTO data)
        {
            log.Info(data.Result);
        } 

        public TaskFindRecords()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(HostingEnvironment.MapPath("~/log4net.config")));
            processData = new DataProc(processData);
        }
        public string personId { get; set; }
        public string length { get; set; }
        public string index { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }

        public void fill()
        {
            pass = "123456";
            interfaceName = "findRecords";
            personId = "-1";
            startTime = "0";
            length = "-1";
            index = "0";
            endTime = "2019-05-06 01:00:05";
            result = true;
        }
    }
}