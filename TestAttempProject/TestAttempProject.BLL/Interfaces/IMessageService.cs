using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAttemptProject.Common.Entities;
using TestAttemptProject.Common.DTO;

namespace TestAttemptProject.BLL.Interfaces
{
    public interface IMessageService
    {
        public Task AddMessageToDbAsync(MessageCreateDTO message, string userIdentityName);
        public Task<Message> GetMessageAsync(int id, string userIdentityName);
        public Task<IEnumerable<Message>> GetAllMessages(string userIdentityName);
        public Task UpdateMessageAsync(MessageUpdateDTO message, string userIdentityName);
        public Task DeleteMessageAsync(int id, string userIdentityName);
    }
}
