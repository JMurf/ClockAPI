
namespace TimeClock.DTOs
{
    public class PunchRequestDTO
    {
        public string DeviceKey { get; set; }   /* which clock */
        public string StartTime { get; set; }   /* punches >= to this datetime */
        public string EndTime { get; set; }     /* punches <= to this datetime */
        public string PersonId { get; set; }       /* employee id, -1 equals all employees */
        public string Length { get; set; }       /* employee id, -1 equals all employees */
        public string Index { get; set; }       /* employee id, -1 equals all employees */
    }
}