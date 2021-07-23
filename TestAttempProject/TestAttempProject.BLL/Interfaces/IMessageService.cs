using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAttemptProject.Domain.Entities;
using TestAttemptProject.Domain.DTO;

namespace TestAttemptProject.BLL.Interfaces
{
    internal interface IMessageService
    {
        public void AddMessageToDb(MessageCreateDTO message);
        public Task<Message> GetMessageAsync(int id);
        public IEnumerable<Message> GetAllMessages();
        public void UpdateMessage(MessageUpdateDTO message);
        public void DeleteMessage(int id);
    }
}
