namespace TimeClock.DTOs
{
    public class PunchRecordDTO:RecordDTO
    {
        public string EmployeeId { get; set; }
        public string PunchDate { get; set; }
    }
}