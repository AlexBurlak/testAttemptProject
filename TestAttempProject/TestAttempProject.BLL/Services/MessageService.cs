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
        public void AddMessageToDb(MessageCreateDTO messageDTO)
        {
            Message message = _mapper.Map<Message>(messageDTO);
            _messageRepository.AddAsync(message);
        }

        public IEnumerable<Message> GetAllMessages()
        {
            return _messageRepository.GetAll();
        }

        public async Task<Message> GetMessageAsync(int id)
        {
            return await _messageRepository.GetAsync(id);
        }

        public void UpdateMessage(MessageUpdateDTO messageDTO)
        {
            Message message = _mapper.Map<Message>(messageDTO);
            _messageRepository.Update(message);
        }
        public void DeleteMessage(int id)
        {
            _messageRepository.Delete(id);
        }
    }
}
