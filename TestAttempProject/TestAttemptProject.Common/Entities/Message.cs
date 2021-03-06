using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAttemptProject.Common.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Content { get; set; }
        public DateTime CreationStamp { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
