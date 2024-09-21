using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HomemadeCakes.DAL.MogoDB.ObjectContext
{
    public class ErrorResult
    {
        public int AddedErrorCount { get; set; }

        public int UpdatedErrorCount { get; set; }

        public int DeletedErrorCount { get; set; }

        public IEnumerable<ActionResult> AddedErrors { get; set; }

        public IEnumerable<ActionResult> UpdatedErrors { get; set; }

        public IEnumerable<ActionResult> DeletedErrors { get; set; }

        public ErrorResult()
        {
            AddedErrors = new List<ActionResult>();
            UpdatedErrors = new List<ActionResult>();
            DeletedErrors = new List<ActionResult>();
        }

        public static void CreateErrorResult(IList<ActionResult> addedResult, IList<ActionResult> updatedResult, IList<ActionResult> deletedResult, string collectionName, IDictionary<string, ErrorResult> errorResults)
        {
            if (addedResult.Count != 0 || updatedResult.Count != 0 || deletedResult.Count != 0)
            {
                ErrorResult value = new ErrorResult
                {
                    AddedErrors = addedResult,
                    UpdatedErrors = updatedResult,
                    DeletedErrors = deletedResult,
                    AddedErrorCount = addedResult.Count,
                    UpdatedErrorCount = updatedResult.Count,
                    DeletedErrorCount = deletedResult.Count
                };
                errorResults.Add(collectionName, value);
            }
        }
    }

}
