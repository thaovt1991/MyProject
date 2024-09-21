using System.Collections.Generic;
using System;

namespace HomemadeCakes.Common.Http
{
    public class MessageBase : IDisposable
    {
        private bool _disposed;

        public LVError Error { get; set; }

        public List<object> MsgBodyData { get; set; }

        public bool HaveBusinessError { get; set; }

        public bool HaveServerError { get; set; }

        public MessageBase()
        {
            MsgBodyData = new List<object>();
        }

        public MessageBase(MessageBase data)
        {
            if (data != null)
            {
                MsgBodyData = new List<object>();
                Error = data.Error?.Clone();
                List<object> msgBodyData = data.MsgBodyData;
                if (msgBodyData != null && msgBodyData.Count > 0)
                {
                    MsgBodyData = data.MsgBodyData.ToList();
                }

                HaveBusinessError = data.HaveBusinessError;
                HaveServerError = data.HaveServerError;
            }
        }

        ~MessageBase()
        {
            Dispose(disposing: false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (MsgBodyData != null)
                {
                    MsgBodyData.Clear();
                    MsgBodyData = null;
                }

                if (Error != null)
                {
                    Error.Dispose();
                    Error = null;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public virtual MessageBase Clone()
        {
            return new MessageBase(this);
        }
    }
}
