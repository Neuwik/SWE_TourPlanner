using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_TourPlanner_WPF.BusinessLayer
{
    //https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
    public class BusinessLayerException : Exception
    {
        public int ErrorCode { get; init; } = 500;
        public string ErrorMessage { get; init; } = "Internal Server Error";

        public BusinessLayerException() : base() { }

        public BusinessLayerException(string message) : base(message) { }

        protected BusinessLayerException(int errorCode, string errorMessage) : base()
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        protected BusinessLayerException(int errorCode, string errorMessage, string message) : base(message)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            if (Message != null)
                return $"BLL - {ErrorCode} {ErrorMessage}: {Message}";
            return $"BLL - {ErrorCode} {ErrorMessage}";
        }
    }

    public class BLLNotImplementedException : BusinessLayerException
    {
        public BLLNotImplementedException() : base(501, "Not Implemented") { }
        public BLLNotImplementedException(string message) : base(501, "Not Implemented", message) { }
    }

    public class BLLConflictException : BusinessLayerException
    {
        public BLLConflictException() : base(409, "Conflict") { }
        public BLLConflictException(string message) : base(409, "Conflict", message) { }
    }

    public class BLLServiceUnavailable : BusinessLayerException
    {
        public BLLServiceUnavailable() : base(503, "Service Unavailable") { }
        public BLLServiceUnavailable(string message) : base(503, "Service Unavailable", message) { }
    }
}
