using System;

namespace HomemadeCakes.Model.Common
{
    public class ResponseBase<T> : ErrorResponse
    {
        public T Data { get; set; }

    }
    #region Error
    public class ErrorResponse
    {
        public bool Error { get; set; } = false;
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

    }

    #endregion

    #region token
    public class LoginResponse : ErrorResponse
    {
        public string Token { get; set; }
        public DateTime Expiress { get; set; }
        public long Maxage { get; set; }
        public string UserID { get; set; }

    }
    #endregion
}
