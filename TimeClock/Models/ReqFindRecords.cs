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
        public void ProcessFindRecords(ClockDTO data)
        {
            log.Info(data);
            Data = JsonConvert.SerializeObject(data);
        }

        public RequestFindRecords()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(HostingEnvironment.MapPath("~/log4net.config")));
            ProcessData = new DataProc(ProcessFindRecords);
        }
        public string PersonId { get; set; }
        public string Length { get; set; }
        public string Index { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}