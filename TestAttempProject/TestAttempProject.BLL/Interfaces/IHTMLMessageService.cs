using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAttemptProject.Common.Entities;
using TestAttemptProject.Common.DTO;

namespace TestAttemptProject.BLL.Interfaces
{
    public interface IHTMLMessageService
    {
        public Task AddMessageToDbAsync(HTMLMessageCreateDTO message, string userIdentityName);
        public Task<HTMLMessage> GetMessageAsync(int id, string userIdentityName);
        public Task<IEnumerable<HTMLMessage>> GetAllMessages(string userIdentityName);
        public Task UpdateMessageAsync(HTMLMessageUpdateDTO message, string userIdentityName);
        public Task DeleteMessageAsync(int id, string userIdentityName);
    }
}
