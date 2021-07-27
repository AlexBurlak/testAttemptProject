using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAttemptProject.Common.Exceptions
{
    public class AccesForbidenException : Exception
    {
        public AccesForbidenException()
            : base()
        {

        }
        public AccesForbidenException(string message)
            : base(message)
        { }

        public AccesForbidenException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}