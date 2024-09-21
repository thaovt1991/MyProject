using System.Collections.Generic;
using System;

namespace HomemadeCakes.DAL.MogoDB.ObjectContext
{
    public class ExceptionResult : Exception
    {
        public bool HasError { get; set; }

        public int AddedCount { get; set; }

        public int UpdatedCount { get; set; }

        public int DeletedCount { get; set; }

        public IDictionary<string, ErrorResult> ErrorResults { get; set; }

        public ExceptionResult(string errorMessage)
            : base(errorMessage)
        {
            ErrorResults = new Dictionary<string, ErrorResult>();
        }
    }
}
