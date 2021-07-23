using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAttemptProject.DAL.Interfaces;
using TestAttemptProject.Domain.Entities;
using TestAttemptProject.Domain.Exceptions;
using TestAttemptProject.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace TestAttemptProject.DAL.Realization
{
    class MessageRepository : IMessageRepository
    {
        private readonly TestAttemptDbContext _context;
        public MessageRepository(TestAttemptDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Message item)
        {
            await _context.Messages.AddAsync(item);
        }

        public void Delete(int messageId)
        {
            Message message = _context.Messages.Find(messageId);
            if(message == null)
            {
                throw new BaseException($"There is no message with id {messageId} in database.");
            }
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public  IEnumerable<Message> GetAll()
        {
            return _context.Messages;
        }

        public void Update(Message item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
