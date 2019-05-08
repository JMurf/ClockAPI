using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeClock.DTOs
{
    public class ReturnDTO
    {
        public string Data { get; set; }
        public string Result { get; set; }
        public string Success { get; set; }
        public string Msg { get; set; }

        public override string ToString()
        {
            string retStr = "";
            if( Data != null ) retStr += "\nData: " + Data;
            if( Result != null ) retStr = retStr + "\nResult: " + Result;
            if( Msg != null ) retStr = retStr + "\nMsg: " + Msg;
            if( Success != null ) retStr = retStr + "\nSuccess: " + Success;
            return retStr;
        }
    }
}