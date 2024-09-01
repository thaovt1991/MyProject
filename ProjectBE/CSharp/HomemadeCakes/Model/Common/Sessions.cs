using System;

namespace HomemadeCakes.Model.Common
{
    public class Sessions
    {
        public string SessionsID { get; set; } = DateTime.Now.ToString(); //Cai nay test thoi chu khong dc de vậy nó trung
        public string UserID { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
