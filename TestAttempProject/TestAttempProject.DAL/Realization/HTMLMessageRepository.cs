using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAttemptProject.Common.Entities;
using TestAttemptProject.Common.Exceptions;
using TestAttemptProject.DAL.Context;
using TestAttemptProject.DAL.Interfaces;

namespace TestAttemptProject.DAL.Realization
{
    public class HTMLMessageRepository : IHTMLMessageRepository
    {
        private readonly UserDbContext _context;
        public HTMLMessageRepository(UserDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(HTMLMessage message)
        {
            await _context.HTMLMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int messageId)
        {
            HTMLMessage message = _context.HTMLMessages.Find(messageId);
            if (message == null)
            {
                throw new MessageNotFoundException($"There is no message with id {messageId} in database.");
            }
            _context.HTMLMessages.Remove(message);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<HTMLMessage> GetAll()
        {
            return _context.HTMLMessages.Include(m => m.Author);
        }

        public async Task<HTMLMessage> GetAsync(int id)
        {
            var message =  await _context.HTMLMessages.Include(m => m.Author).FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                throw new MessageNotFoundException($"There is no message with id {id} in database.");
            }
            return message;
        }
        public async Task UpdateAsync(HTMLMessage item)
        {
            HTMLMessage oldMessage = await _context.HTMLMessages.FindAsync(item.Id);
            if (oldMessage == null) { throw new MessageNotFoundException($"There is no message with id {item.Id} in database."); }
            oldMessage.Content = item.Content;
            oldMessage.EditDate = item.EditDate;
            await _context.SaveChangesAsync();
        }
    }
}
