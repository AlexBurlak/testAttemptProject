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
        }

        public async Task DeleteAsync(int messageId)
        {
            HTMLMessage message = _context.HTMLMessages.Find(messageId);
            if (message == null)
            {
                throw new BaseException($"There is no message with id {messageId} in database.");
            }
            _context.HTMLMessages.Remove(message);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<HTMLMessage> GetAll()
        {
            return _context.HTMLMessages;
        }

        public async Task<HTMLMessage> GetAsync(int id)
        {
            return await _context.HTMLMessages.FindAsync(id);
        }
        public async Task UpdateAsync(HTMLMessage item)
        {
            HTMLMessage oldMessage = await _context.HTMLMessages.FindAsync(item);
            if (oldMessage == null) { throw new BaseException(); }
            oldMessage.Content = item.Content;
            oldMessage.EditDate = item.EditDate;
            await _context.SaveChangesAsync();
        }
    }
}
