using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using TimeClock.DTOs;

namespace TimeClock.Models
{
    public class ReqFindRecords : ReqClock
    {
        [JsonIgnore]
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /* delegate */
        public List<RecordDTO> processFindRecords(PunchClockDTO data)
        {
            log.Info(data);
            List<RecordDTO> list = new List<RecordDTO>();
            PunchRecordDTO pr = new PunchRecordDTO();
            pr.EmployeeId = "12345A";
            list.Add(pr);
            pr = new PunchRecordDTO();
            pr.EmployeeId = "54321B";
            list.Add(pr);
            return list;
        }

        public ReqFindRecords()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(HostingEnvironment.MapPath("~/log4net.config")));
            ProcessData = new DataProc(processFindRecords);
        }
        public string personId { get; set; }
        public string length { get; set; }
        public string index { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }

        public List<PunchRecordDTO> getData()
        {
            return new List<PunchRecordDTO>();
        }
        public void fill()
        {
            Pass = "123456";
            InterfaceName = "findRecords";
            personId = "-1";
            startTime = "0";
            length = "-1";
            index = "0";
            endTime = "2019-05-06 01:00:05";
            Result = true;
        }
    }
}