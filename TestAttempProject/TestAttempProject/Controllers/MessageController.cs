using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAttemptProject.BLL.Interfaces;
using TestAttemptProject.Common.Entities;
using TestAttemptProject.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TestAttemptProject.Controllers
{
    
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly UserManager<User> _userManager;
        public MessageController(IMessageService messageService,
            UserManager<User> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }
        
        [HttpGet]
        public  async Task<ActionResult<IEnumerable<Message>>> GetAll()
        {
            IEnumerable<Message> messagesCollection = _messageService.GetAllMessages();
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if(!isAdmin)
            {
                messagesCollection = messagesCollection.Where(m => m.Author == user);
            }
            return  Ok(messagesCollection);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetAsync(int id)
        {
            var message = await _messageService.GetMessageAsync(id);
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if(!isAdmin && message.Author != user)
            {
                return Forbid();
            }
            return Ok(message);
        }

        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MessageCreateDTO messageDTO)
        {
            await _messageService.AddMessageToDbAsync(messageDTO, await _userManager.FindByNameAsync(User.Identity.Name));
            return Created(nameof(GetAsync), messageDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MessageUpdateDTO messageDTO)
        {
            if ((await _messageService.GetMessageAsync(id)).Author !=
                (await _userManager.FindByNameAsync(User.Identity.Name)))
            {
                return Forbid();
            }
            await _messageService.UpdateMessageAsync(id, messageDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var message = await _messageService.GetMessageAsync(id);
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            bool isAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            if(!isAdmin && message.Author != user)
            {
                return Forbid();
            }
            await _messageService.DeleteMessageAsync(id);
            return NoContent();
        }
        
    }
}
