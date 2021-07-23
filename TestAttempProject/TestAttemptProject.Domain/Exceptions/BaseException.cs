using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAttemptProject.Domain.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException()
            :base()
        {

        }
        public BaseException(string message)
            : base(message)
        { }

        public BaseException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
