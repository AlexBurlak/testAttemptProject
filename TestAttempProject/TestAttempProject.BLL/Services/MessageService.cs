using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAttemptProject.BLL.Interfaces;
using TestAttemptProject.Common.Exceptions;
using TestAttemptProject.Common.DTO;
using TestAttemptProject.Common.Entities;
using TestAttemptProject.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace TestAttemptProject.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMapper _mapper;
        private readonly IMessageRepository _messageRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public MessageService(IMapper mapper,
            IMessageRepository messageRepository,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _messageRepository = messageRepository;
        }
        public async Task AddMessageToDbAsync(MessageCreateDTO messageDTO, string userIdentityName)
        {
            var user = await _userManager.FindByNameAsync(userIdentityName);
            Message message = _mapper.Map<Message>(messageDTO);
            message.Author = user;
            await _messageRepository.AddAsync(message);
        }

        public async Task<IEnumerable<Message>> GetAllMessages(string userIdentityName)
        {
            var messagesCollection = _messageRepository.GetAll();
            User user = await _userManager.FindByNameAsync(userIdentityName);
            bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if (!isAdmin)
            {
                messagesCollection = messagesCollection.Where(m => m.Author == user);
            }
            return messagesCollection;
        }

        public async Task<Message> GetMessageAsync(int id, string userIdentityName)
        {
            var message = await _messageRepository.GetAsync(id);
            User user = await _userManager.FindByNameAsync(userIdentityName);
            bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if (!isAdmin && message.Author != user)
            {
                throw new BaseException();
            }
            return message;
        }

        public async Task UpdateMessageAsync(MessageUpdateDTO messageDTO, string userIdentityName)
        {
            var user = await _userManager.FindByNameAsync(userIdentityName);
            var author = (await _messageRepository.GetAsync(messageDTO.Id)).Author;
            if (author != user)
            {
                throw new BaseException();
            }
            Message message = _mapper.Map<Message>(messageDTO);
            message.EditTime = DateTime.Now;
            await _messageRepository.UpdateAsync(message);
        }
        public async Task DeleteMessageAsync(int id, string userIdentityName)
        {
            var message = await _messageRepository.GetAsync(id);
            User user = await _userManager.FindByNameAsync(userIdentityName);
            bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if (!isAdmin && message.Author != user)
            {
                throw new BaseException();
            }
            await _messageRepository.DeleteAsync(id);
        }
    }
}
