using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestAttemptProject.BLL.Interfaces;
using TestAttemptProject.Common.DTO;
using TestAttemptProject.Common.Entities;
using TestAttemptProject.Common.Exceptions;
using TestAttemptProject.DAL.Interfaces;

namespace TestAttemptProject.BLL.Services
{
    public class HTMLMessageService : IHTMLMessageService
    {
        private readonly IHTMLMessageRepository _htmlMessageRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public HTMLMessageService(IHTMLMessageRepository htmlMessageRepository,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _htmlMessageRepository = htmlMessageRepository;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task AddMessageToDbAsync(HTMLMessageCreateDTO message, string userIdentityName)
        {
            if (message.Content == null)
            {
                throw new BaseException("Message content can't be null");
            }

            var user = await _userManager.FindByNameAsync(userIdentityName);

            HTMLMessage htmlMessage = _mapper.Map<HTMLMessage>(message);
            if (!IsTextHtml(htmlMessage.Content)) throw new HTMLMessageError("Text doesn't contain any html tag. Use message controller instead htmlmessage.");
            htmlMessage.Author = user;
            htmlMessage.DataStamp = DateTime.Now;
            await _htmlMessageRepository.AddAsync(htmlMessage);
        }
        private bool IsTextHtml(string content)
        {
            Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
            Regex tagRegexOneBracket = new Regex(@"<[^>]+>");
            bool result = tagRegex.IsMatch(content) ? true : tagRegexOneBracket.IsMatch(content);
            return result;
        }
        public async Task DeleteMessageAsync(int id, string userIdentityName)
        {
            var message = await _htmlMessageRepository.GetAsync(id);
            if(message == null)
            {
                throw new MessageNotFoundException($"There is no message with id {id} in database.");
            }
            var user = await _userManager.FindByNameAsync(userIdentityName);
            bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if(!isAdmin && message.Author != user)
            {
                throw new AccesForbidenException("Only author can delete this message!");
            }
            await _htmlMessageRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<HTMLMessage>> GetAllMessages(string userIdentityName)
        {
            var messages = _htmlMessageRepository.GetAll();
            var user = await _userManager.FindByNameAsync(userIdentityName);
            bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if(!isAdmin)
            {
                messages = messages.Where(msg => msg.Author == user).ToList();
            }
            return messages;
        }

        public async Task<HTMLMessage> GetMessageAsync(int id, string userIdentityName)
        {
            var message = await _htmlMessageRepository.GetAsync(id);
            var user = await _userManager.FindByNameAsync(userIdentityName);
            bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if(!isAdmin && message.Author != user)
            {
                throw new AccesForbidenException("Only author can get this message!");
            }
            return message;
        }

        public async Task UpdateMessageAsync(HTMLMessageUpdateDTO message, string userIdentityName)
        {
            if (message.Content == null)
            {
                throw new BaseException("Message content can't be null");
            }

            var user = await _userManager.FindByNameAsync(userIdentityName);
            bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);

            HTMLMessage oldMessage = await _htmlMessageRepository.GetAsync(message.Id);
            if (oldMessage == null) throw new MessageNotFoundException($"There is no message with id {message.Id} in database.");

            if (!isAdmin && oldMessage.Author != user) throw new AccesForbidenException("Only author can update this message!");

            HTMLMessage updatedMessage = _mapper.Map<HTMLMessage>(message);
            if (!IsTextHtml(updatedMessage.Content)) throw new HTMLMessageError("Text doesn't contain any html tag. Use message controller instead htmlmessage.");
            updatedMessage.EditDate = DateTime.Now;

            await _htmlMessageRepository.UpdateAsync(updatedMessage);
        }
    }
}
