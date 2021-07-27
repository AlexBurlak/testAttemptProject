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
        public IEnumerable<HTMLMessageCreateDTO> Get()
        {
            return _messageService.GetAllMessages();
        }

        [HttpGet("{id}")]
        public async Task<HTMLMessageCreateDTO> Get(int id)
        {
            return await _messageService.GetMessageAsync(id);
        }

        [HttpPost]
        public async Task Post([FromBody] HTMLMessageCreateDTO messageDTO)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            await _messageService.AddMessageToDbAsync(messageDTO, user);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] HTMLMessageCreateDTO messageDTO)
        {
            await _messageService.UpdateMessageAsync(id, messageDTO);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _messageService.DeleteMessageAsync(id);
        }
    }
}
