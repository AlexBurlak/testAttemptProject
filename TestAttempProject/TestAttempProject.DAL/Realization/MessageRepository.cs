using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAttemptProject.DAL.Interfaces;
using TestAttemptProject.Common.Entities;
using TestAttemptProject.Common.Exceptions;
using TestAttemptProject.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace TestAttemptProject.DAL.Realization
{
    public class MessageRepository : IMessageRepository
    {
        private readonly UserDbContext _context;
        public MessageRepository(UserDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Message item)
        {
            await _context.Messages.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int messageId)
        {
            Message message = await _context.Messages.FindAsync(messageId);
            if(message == null)
            {
                throw new BaseException($"There is no message with id {messageId} in database.");
            }
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }

        public async Task<Message> GetAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public  IEnumerable<Message> GetAll()
        {
            return _context.Messages;
        }

        public async Task UpdateAsync(Message item)
        {
            Message message = _context.Messages.FirstOrDefault(ms => ms.Id == item.Id);
            if(message == null) { throw new BaseException(); }
            message.Content = item.Content;
            message.EditTime = item.EditTime;
            await _context.SaveChangesAsync();
        }
    }
}
