namespace HomemadeCakes.Common
{
    public static class ErrorTracker
    {
        public static PJError CreateError(string sMsg = null)
        {
            return new PJError
            {
                ErrorMessage = sMsg,
                IsError = false
            };
        }

        public static PJBusinessException CreateException(PJError error)
        {
            return new PJBusinessException
            {
                Error = error
            };
        }
    }
}
