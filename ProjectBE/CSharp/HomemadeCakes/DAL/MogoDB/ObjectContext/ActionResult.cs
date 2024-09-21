using System;

namespace HomemadeCakes.DAL.MogoDB.ObjectContext
{
    public class ActionResult : Exception
    {
        public object ObjectData { get; set; }

        public ActionResult()
        {
        }

        public ActionResult(string errorMessage)
            : base(errorMessage)
        {
        }

        public ActionResult(string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
        }

        public ActionResult(string errorMessage, Exception innerException, object objData)
            : base(errorMessage, innerException)
        {
            ObjectData = objData;
        }
    }

}
