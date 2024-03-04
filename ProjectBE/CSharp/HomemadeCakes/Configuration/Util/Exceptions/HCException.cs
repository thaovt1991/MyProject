using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomemadeCakes.Configuration.Util.Exceptions
{
    public class HCException : Exception
    {
        public HCException()
        {

        }
        public HCException(string mess) : base (mess)
        {

        }
        public HCException (string mess , Exception ex) : base (mess , ex)
        {

        }
    }
}
