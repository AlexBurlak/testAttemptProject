using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAttemptProject.Domain.Entities;
using TestAttemptProject.Domain.DTO;

namespace TestAttemptProject.BLL.Interfaces
{
    public interface IMessageService
    {
        public Task AddMessageToDbAsync(MessageCreateDTO message);
        public Task<Message> GetMessageAsync(int id);
        public IEnumerable<Message> GetAllMessages();
        public Task UpdateMessageAsync(int id, MessageUpdateDTO message);
        public Task DeleteMessageAsync(int id);
    }
}
