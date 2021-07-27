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
        public Task AddMessageToDbAsync(HTMLMessageCreateDTO message, User user);
        public Task<HTMLMessageCreateDTO> GetMessageAsync(int id);
        public IEnumerable<HTMLMessageCreateDTO> GetAllMessages();
        public Task UpdateMessageAsync(HTMLMessageUpdateDTO message);
        public Task DeleteMessageAsync(int id);
    }
}
