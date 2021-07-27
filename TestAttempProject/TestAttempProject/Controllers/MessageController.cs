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
using TestAttemptProject.Common.Exceptions;
using System.Text.RegularExpressions;

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
            IEnumerable<Message> messagesCollection = await _messageService.GetAllMessages(User.Identity.Name);
            return  Ok(messagesCollection);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetAsync(int id)
        {
            try { 
                var message = await _messageService.GetMessageAsync(id, User.Identity.Name);
                return Ok(message);
            }
            catch (AccesForbidenException ex)
            {
                return Forbid(ex.Message);
            }
            catch (MessageNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MessageCreateDTO messageDTO)
        {
            try { 
                await _messageService.AddMessageToDbAsync(messageDTO, User.Identity.Name);
                return Created(nameof(GetAsync), messageDTO);
            }
            catch (BaseException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<ActionResult> Put([FromBody] MessageUpdateDTO messageDTO)
        {
            try { 
                await _messageService.UpdateMessageAsync(messageDTO, User.Identity.Name);
                return Ok();
            }
            catch (AccesForbidenException ex)
            {
                return Forbid(ex.Message);
            }
            catch (MessageNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BaseException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try 
            {    
                await _messageService.DeleteMessageAsync(id, User.Identity.Name);
                return NoContent();
            }
            catch (AccesForbidenException ex)
            {
                return Forbid(ex.Message);
            }
            catch (MessageNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
    }
}
