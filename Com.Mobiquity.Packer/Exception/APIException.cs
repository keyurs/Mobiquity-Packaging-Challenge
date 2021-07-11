using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Mobiquity.Packer
{

    [Serializable]
    public class APIException : Exception
    {
        private String lineData;
        private int lineNumber;

        public APIException() { }

        public APIException(Exception e, String lineData, int lineNumber)
            : base(String.Format("Error in line : {0} - Data {1} - Error details : {2}", lineNumber, lineData, e.Message))
        {
            this.lineData = lineData;
            this.lineNumber = lineNumber;
        }

        public APIException(String ErrorDetail, String lineData, int lineNumber)
            : base(String.Format("Error in line : {0} - Data {1} - Error details : {2}", lineNumber, lineData, ErrorDetail))
        {
            this.lineData = lineData;
            this.lineNumber = lineNumber;
        }
    }
}
