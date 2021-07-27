using AutoMapper;
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
        public HTMLMessageService(IHTMLMessageRepository htmlMessageRepository,
            IMapper mapper)
        {
            _htmlMessageRepository = htmlMessageRepository;
            _mapper = mapper;
        }
        public async Task AddMessageToDbAsync(HTMLMessageCreateDTO message, User user)
        {
            HTMLMessage htmlMessage = _mapper.Map<HTMLMessage>(message);
            if (!IsTextHtml(htmlMessage.Content)) throw new BaseException();
            htmlMessage.Author = user;
            htmlMessage.DataStamp = DateTime.Now;
            await _htmlMessageRepository.AddAsync(htmlMessage);
        }
        private bool IsTextHtml(string content)
        {
            Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
            return tagRegex.IsMatch(content);
        }
        public async Task DeleteMessageAsync(int id)
        {
            if((await _htmlMessageRepository.GetAsync(id)) == null)
            {
                throw new BaseException();
            }
            await _htmlMessageRepository.DeleteAsync(id);
        }

        public IEnumerable<HTMLMessageCreateDTO> GetAllMessages()
        {
            return _htmlMessageRepository.GetAll()
                .Select(m => _mapper.Map<HTMLMessageCreateDTO>(m))
                .ToList();
        }

        public async Task<HTMLMessageCreateDTO> GetMessageAsync(int id)
        {
            return _mapper.Map<HTMLMessageCreateDTO>((await _htmlMessageRepository.GetAsync(id)));
        }

        public async Task UpdateMessageAsync(HTMLMessageUpdateDTO message)
        {
            HTMLMessage oldMessage = await _htmlMessageRepository.GetAsync(message.Id);
            if (oldMessage == null)  throw new BaseException();
            HTMLMessage updatedMessage = _mapper.Map<HTMLMessage>(message);
            updatedMessage.Author = oldMessage.Author;
            updatedMessage.EditDate = oldMessage.EditDate;
            await _htmlMessageRepository.UpdateAsync(updatedMessage);
        }
    }
}
