using HomemadeCakes.Common.Http;
using HomemadeCakes.Common;
using System.IO;
using System.Threading;
using System;
using HomemadeCakes.Common.Config;
using HomemadeCakes.Common.Interface;
using HomemadeCakes.DAL.Interface;
using HomemadeCakes.DAL.Base;
using HomemadeCakes.Common.EventBus.Abstractions;


namespace HomemadeCakes.DAL
{
    public class PJRequestContext : IDisposable
    {
        public static readonly string DBDefultType = "PostgreSQL";//cop từ project qua tự học sau

        public DALContainerStorage DALContainerStorage = new DALContainerStorage();

        public PJStreamReader StreamReader;

        public PJStreamWriter StreamWriter;

        public IEventBus EvenBus;

        public dynamic HubContext;

        public PJError Error;

        public IDALContainer DALContainer;

        public MessageBody RequestSession => StreamReader;

        public IConnection RequestConnection => DALContainer?.Connection;

        public string Language
        {
            get
            {
                string text = StreamReader?.Language;
                if (string.IsNullOrEmpty(text))
                {
                    text = PJConfig.Settings.Language;
                }

                return text;
            }
        }

        public static PJRequestContext Current
        {
            get
            {
                if (Thread.CurrentPrincipal == null)
                {
                    return ((DALPrincipal)(Thread.CurrentPrincipal = new DALPrincipal())).RequestContext;
                }

                if (Thread.CurrentPrincipal is IDALPrincipal iDALPrincipal)
                {
                    return iDALPrincipal.RequestContext;
                }

                return ((DALPrincipal)(Thread.CurrentPrincipal = new DALPrincipal(Thread.CurrentPrincipal))).RequestContext;
            }
        }

        public void Dispose()
        {
        }

        public static PJRequestContext Init(MessageBody request, bool createDbContext = true)
        {
            PJRequestContext current = Current;
            current.StreamWriter = new PJStreamWriter();
            current.StreamReader = new PJStreamReader(request);
            if (string.IsNullOrEmpty(request.SessionKey))
            {
                current.StreamReader.SessionKey = Guid.NewGuid().ToString();
            }

            current.Error = ErrorTracker.CreateError(request.FormName);
            if (createDbContext)
            {
                if (request.ConnectionDB == null)
                {
                    if (string.IsNullOrEmpty(request.ConnectionName))
                    {
                        current.StreamReader.ConnectionDB = null;
                        current.StreamReader.SecurityKey = null;
                        current.StreamReader.Token = null;
                        current.StreamReader.Dispose();
                        current.Error = new PJError
                        {
                            IsError = true,
                            ErrorCode = "404",
                            ErrorMessage = "DB not found"
                        };
                        return null;
                    }

                    Type containerType = DALContainerManager.ContainerType;
                    string text = (((object)containerType != null && containerType.FullName?.Contains("MongoDB") == true) ? "MongoDB" : (PJConfig.Settings.DBNameType ?? DBDefultType));
                    string cnnString = "";
                    if (!string.IsNullOrEmpty(text))
                    {
                        cnnString = PJConfig.Connections[text];
                    }

                    request.ConnectionDB = new ConnectionDB
                    {
                        DBName = request.ConnectionName,
                        CnnString = cnnString,
                        Type = text,
                        IsConfig = true
                    };
                }

                current.DALContainer = DALContainerManager.CreateDALContainer(request.ConnectionDB);
                current.DALContainer.UnitOfWork.BUID = request.BUID;
                current.DALContainer.UnitOfWork.UserID = request.UserID;
                request.ConnectionDB = current.DALContainer.Connection;
                if (string.IsNullOrEmpty(request.DbSytem))
                {
                    request.DbSytem = request.ConnectionName;
                }

                if (current.StreamReader != null)
                {
                    current.StreamReader.ConnectionDB = request.ConnectionDB;
                    current.StreamReader.DbSytem = request.DbSytem;
                }
            }

            return current;
        }
    }

}
