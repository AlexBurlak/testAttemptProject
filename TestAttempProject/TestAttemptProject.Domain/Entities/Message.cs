using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAttemptProject.Domain.Entities
{
    class Message
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Content { get; set; }
        public DateTime dataStamp { get; set; }
    }
}
