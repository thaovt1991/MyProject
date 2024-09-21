using System;

namespace HomemadeCakes.Common
{
    public class PJBusinessException : Exception
    {
        private PJError _error;

        public PJError Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
            }
        }

        public PJBusinessException()
        {
        }

        public PJBusinessException(string error)
            : base(error)
        {
        }
    }
}
