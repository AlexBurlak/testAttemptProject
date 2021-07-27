using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAttemptProject.Common.Exceptions
{
    public class HTMLMessageError : Exception
    {
        public HTMLMessageError()
            : base()
        {

        }
        public HTMLMessageError(string message)
            : base(message)
        { }

        public HTMLMessageError(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
