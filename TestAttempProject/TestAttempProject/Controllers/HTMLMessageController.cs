using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAttemptProject.BLL.Interfaces;
using TestAttemptProject.Common.DTO;
using TestAttemptProject.Common.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestAttemptProject.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class HTMLMessageController : ControllerBase
    {
        private readonly IHTMLMessageService _messageService;
        private readonly UserManager<User> _userManager;
        public HTMLMessageController(IHTMLMessageService messageService, 
            UserManager<User> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<HTMLMessage>> Get()
        {
            return await _messageService.GetAllMessages(User.Identity.Name);
        }

        [HttpGet("{id}")]
        public async Task<HTMLMessage> Get(int id)
        {
            return await _messageService.GetMessageAsync(id, User.Identity.Name);
        }

        [HttpPost]
        public async Task Post([FromBody] HTMLMessageCreateDTO messageDTO)
        {
            await _messageService.AddMessageToDbAsync(messageDTO, User.Identity.Name);
        }

        [HttpPut]
        public async Task Put([FromBody] HTMLMessageUpdateDTO messageDTO)
        {
            await _messageService.UpdateMessageAsync(messageDTO, User.Identity.Name);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _messageService.DeleteMessageAsync(id, User.Identity.Name);
        }
    }
}
