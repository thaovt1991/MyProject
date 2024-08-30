namespace HomemadeCakes.Model.Common
{
    public class ResponseBase<T> : ErrorResponse
    {
        public T Data { get; set; }

    }
    #region Error
    public class ErrorResponse
    {
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

    }

    #endregion

    #region token
    public class TokenResponse : ErrorResponse
    {
        public string Token { get; set; }
        public long Expiress { get; set; }
        public string UserID { get; set; }

    }
    #endregion
}
