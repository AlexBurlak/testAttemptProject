using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAttemptProject.BLL.Interfaces;
using TestAttemptProject.Domain.DTO;
using TestAttemptProject.Domain.Entities;
using TestAttemptProject.DAL.Interfaces;
using TestAttemptProject.Domain.Exceptions;

namespace TestAttemptProject.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;
        public MessageService(IMapper mapper,
            IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _messageRepository = messageRepository;
        }
        public async Task AddMessageToDbAsync(MessageCreateDTO messageDTO)
        {
            Message message = _mapper.Map<Message>(messageDTO);
            message.DataStamp = DateTime.Now;
            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveAsync();
        }

        public IEnumerable<Message> GetAllMessages()
        {
            return _messageRepository.GetAll();
        }

        public async Task<Message> GetMessageAsync(int id)
        {
            return await _messageRepository.GetAsync(id);
        }

        public async Task UpdateMessageAsync(int id, MessageUpdateDTO messageDTO)
        {
            Message message = _mapper.Map<Message>(messageDTO);
            message.Id = id;
            _messageRepository.Update(message);
            await _messageRepository.SaveAsync();
        }
        public async Task DeleteMessageAsync(int id)
        {
            _messageRepository.Delete(id);
            await _messageRepository.SaveAsync();
        }
    }
}
