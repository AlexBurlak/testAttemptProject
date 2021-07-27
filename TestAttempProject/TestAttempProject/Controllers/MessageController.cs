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
        private readonly IHTMLMessageService _htmlMessageService;
        public MessageController(IMessageService messageService,
            IHTMLMessageService hTMLMessageService,
            UserManager<User> userManager)
        {
            _messageService = messageService;
            _htmlMessageService = hTMLMessageService;
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
            catch (BaseException ex)
            {
                return Forbid();
            }
        }

        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MessageCreateDTO messageDTO)
        {
            await _messageService.AddMessageToDbAsync(messageDTO, User.Identity.Name);
            return Created(nameof(GetAsync), messageDTO);
        }

        [HttpPost("html")]
        public async Task<IActionResult> PostHtml([FromBody] HTMLMessageCreateDTO messageCreateDto)
        {
            if (!CheckIsHtmlValid(messageCreateDto.Content))
            {
                return BadRequest();
            }

            await _htmlMessageService.AddMessageToDbAsync(messageCreateDto,
                await _userManager.FindByNameAsync(User.Identity.Name));
            return Ok();
        }   
        private bool CheckIsHtmlValid(string content)
        {
            Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
            return tagRegex.IsMatch(content);
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] MessageUpdateDTO messageDTO)
        {
            try { 
                await _messageService.UpdateMessageAsync(messageDTO, User.Identity.Name);
                return Ok();
            }
            catch (BaseException ex)
            {
                return Forbid();
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
            catch(BaseException ex)
            {
                return Forbid();
            }
        }
        
    }
}
