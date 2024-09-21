using System;
using System.Collections.Generic;
using System.Linq;

namespace HomemadeCakes.Common
{
    public class PJError
    {
        private bool _disposed;

        public static int MaxSubList => 15;

        public bool TypeError { get; set; }

        public bool IsError { get; set; }

        public bool IsSystemError { get; set; }

        public string StackTrace { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string MessageType { get; set; }

        public string ParameterType { get; set; }

        public string EntityName { get; set; }

        public string PropertyName { get; set; }

        public string PropertyValue { get; set; }

        public string MsgIcon { get; set; }

        public IList<PJError> SubErrorList { get; set; }

        public PJError()
        {
            SubErrorList = new List<PJError>();
        }

        ~PJError()
        {
            Dispose(disposing: false);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                SubErrorList = null;
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public PJError Clone()
        {
            if (this == null)
            {
                return null;
            }

            PJError PJError = new PJError();
            Copy(PJError);
            return PJError;
        }

        public void Copy(PJError target)
        {
            if (target != null && this != null)
            {
                target.TypeError = TypeError;
                target.IsError = IsError;
                target.StackTrace = StackTrace;
                target.ErrorCode = ErrorCode;
                target.ErrorMessage = ErrorMessage;
                target.MessageType = MessageType;
                target.ParameterType = ParameterType;
                target.EntityName = EntityName;
                target.PropertyName = PropertyName;
                target.PropertyValue = PropertyValue;
                target.MsgIcon = MsgIcon;
                target.SubErrorList = SubErrorList.ToList();
            }
        }
    }
}
