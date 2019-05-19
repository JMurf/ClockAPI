using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using TimeClock.DTOs;

namespace TimeClock.Models
{
    public class RequestFindRecords : Request
    {
        [JsonIgnore]
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /* delegate */
        public void ProcessFindRecords(PunchClockDTO data)
        {
            log.Info(data);
            PunchList = new List<PunchRecordDTO>();
            PunchRecordDTO pr = new PunchRecordDTO();
            pr.EmployeeId = "12345A";
            PunchList.Add(pr);

            pr = new PunchRecordDTO();
            pr.EmployeeId = "54321B";
            PunchList.Add(pr);
        }

        public RequestFindRecords()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(HostingEnvironment.MapPath("~/log4net.config")));
            ProcessData = new DataProc(ProcessFindRecords);
        }
        public string personId { get; set; }
        public string length { get; set; }
        public string index { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public List<PunchRecordDTO> PunchList { get; set; }

        public List<PunchRecordDTO> getData()
        {
            List<PunchRecordDTO> list = new List<PunchRecordDTO>();


            return list;
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