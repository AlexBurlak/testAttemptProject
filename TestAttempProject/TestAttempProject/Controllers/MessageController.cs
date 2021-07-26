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
        // GET: api/<MessageController>
        [Authorize]
        [HttpGet]
        public  async Task<ActionResult<IEnumerable<Message>>> GetAll()
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            IEnumerable<Message> messagesCollection= _messageService.GetAllMessages();
            if(user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if(!userRoles.Contains(UserRoles.Admin))
                {
                    messagesCollection = messagesCollection.Where(m => m.Author == user);
                }
            }
            return  Ok(messagesCollection);
        }

        // GET api/<MessageController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetAsync(int id)
        {
            return Ok(await _messageService.GetMessageAsync(id));
        }

        // POST api/<MessageController>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MessageCreateDTO messageDTO)
        {
            await _messageService.AddMessageToDbAsync(messageDTO, await _userManager.FindByNameAsync(HttpContext.User.Identity.Name));
            return Created(nameof(GetAsync), messageDTO);
        }

        // PUT api/<MessageController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MessageUpdateDTO messageDTO)
        {
            await _messageService.UpdateMessageAsync(id, messageDTO);
            return Ok();
        }

        // DELETE api/<MessageController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _messageService.DeleteMessageAsync(id);
            return NoContent();
        }
    }
}
