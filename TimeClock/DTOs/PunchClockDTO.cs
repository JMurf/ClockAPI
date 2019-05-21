namespace TimeClock.DTOs
{
    public class ClockDTO
    {
        public string DeviceKey { get; set; }
        public string Time { get; set; }
        public string IP { get; set; }
        public string PersonCount { get; set; }
        public string FaceCount { get; set; }
        public string Version { get; set; }
        public string Data { get; set; }

        public string Result { get; set; }  /* result of task */
        public string InterfaceName { get; set; } /* interface to be executed */
        public int TaskNo { get; set; }  /* task number */

        public override string ToString()
        {
            string retStr = "\n" + base.ToString();
            if( DeviceKey != null ) retStr += "\nDeviceKey: " + DeviceKey;
            if( Time != null ) retStr += "\nTime: " + Time;
            if( IP != null) retStr += "\nIP: " + IP;
            if( PersonCount != null) retStr += "\nPersonCount: " + PersonCount;
            if( FaceCount != null ) retStr += "\nFaceCount: " + FaceCount;
            if( InterfaceName != null ) retStr += "\nInterfaceName: " + InterfaceName;
            if( Version != null )  retStr += "\nVersion: " + Version;
            if( TaskNo != 0 ) retStr += "\nTaskNo: " + TaskNo;
            if( Result != null ) retStr += "\nResult: " + Result;
            return retStr;
        }
    }
}