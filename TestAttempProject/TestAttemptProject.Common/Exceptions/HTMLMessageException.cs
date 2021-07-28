using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAttemptProject.Common.Exceptions
{
    public class HTMLMessageException : Exception
    {
        public HTMLMessageException()
            : base()
        {

        }
        public HTMLMessageException(string message)
            : base(message)
        { }

        public HTMLMessageException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
