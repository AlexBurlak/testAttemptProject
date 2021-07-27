using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAttemptProject.Common.Exceptions
{
    public class MessageNotFoundException : Exception
    {
        public MessageNotFoundException()
            : base()
        {

        }
        public MessageNotFoundException(string message)
            : base(message)
        { }

        public MessageNotFoundException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
