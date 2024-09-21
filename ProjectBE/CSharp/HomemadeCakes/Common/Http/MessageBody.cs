using HomemadeCakes.Common.Interface;
using System.Collections.Generic;
using System.Linq;

namespace HomemadeCakes.Common.Http
{
    public class MessageBody : MessageBase
    {
        private bool _disposed;

        public bool IsRequestJson { get; set; }

        public int RequestTimeout { get; set; }

        public bool BypassValidate { get; set; }

        public string IPClient { get; set; }

        public string DbSytem { get; set; }

        public string UserID { get; set; }

        public string BUID { get; set; }

        public string SessionKey { get; set; }

        public string SecurityKey { get; set; }

        public string Token { get; set; }

        public string Tenant { get; set; }

        public string Language { get; set; }

        public string FunctionID { get; set; }

        public string FormName { get; set; }

        public string GridViewName { get; set; }

        public string EntityName { get; set; }

        public string Service { get; set; }

        public string AssemblyName { get; set; }

        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public int Saas { get; set; }

        public string ConnectionName { get; set; }

        public IConnection ConnectionDB { get; set; }

        public string LocalTz { get; set; }

        public string LocalRegion { get; set; }

        public MessageBody()
        {
            base.MsgBodyData = new List<object>();
        }

        public MessageBody(MessageBody data)
        {
            if (data != null)
            {
                base.MsgBodyData = new List<object>();
                base.Error = data.Error?.Clone();
                List<object> msgBodyData = data.MsgBodyData;
                if (msgBodyData != null && msgBodyData.Count > 0)
                {
                    base.MsgBodyData = data.MsgBodyData.ToList();
                }

                if (data.ConnectionDB != null)
                {
                    ConnectionDB = data.ConnectionDB.Clone();
                }

                base.HaveBusinessError = data.HaveBusinessError;
                base.HaveServerError = data.HaveServerError;
                BypassValidate = data.BypassValidate;
                IPClient = data.IPClient;
                DbSytem = data.DbSytem;
                UserID = data.UserID;
                BUID = data.BUID;
                SessionKey = data.SessionKey;
                SecurityKey = data.SecurityKey;
                Token = data.Token;
                Tenant = data.Tenant;
                Language = data.Language;
                FunctionID = data.FunctionID;
                FormName = data.FormName;
                GridViewName = data.GridViewName;
                EntityName = data.EntityName;
                Service = data.Service;
                AssemblyName = data.AssemblyName;
                ClassName = data.ClassName;
                MethodName = data.MethodName;
                ConnectionName = data.ConnectionName;
                Saas = data.Saas;
                LocalTz = data.LocalTz;
                LocalRegion = data.LocalRegion;
            }
        }

        public MessageBody(MessageBase data)
        {
            if (data != null)
            {
                base.MsgBodyData = new List<object>();
                base.Error = data.Error?.Clone();
                List<object> msgBodyData = data.MsgBodyData;
                if (msgBodyData != null && msgBodyData.Count > 0)
                {
                    base.MsgBodyData = data.MsgBodyData.ToList();
                }

                base.HaveBusinessError = data.HaveBusinessError;
                base.HaveServerError = data.HaveServerError;
            }
        }

        ~MessageBody()
        {
            Dispose(disposing: false);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override MessageBody Clone()
        {
            return new MessageBody(this);
        }
    }
}
