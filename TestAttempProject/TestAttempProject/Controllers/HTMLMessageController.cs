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
using TestAttemptProject.Common.Exceptions;

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
        public async Task<ActionResult<IEnumerable<HTMLMessage>>> GetAsync()
        {
            return Ok(await _messageService.GetAllMessages(User.Identity.Name));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HTMLMessage>> GetAsync(int id)
        {
            try {
                return Ok(await _messageService.GetMessageAsync(id, User.Identity.Name));
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
        public async Task<ActionResult> PostAsync([FromBody] HTMLMessageCreateDTO messageDTO)
        {
            try { 
                await _messageService.AddMessageToDbAsync(messageDTO, User.Identity.Name);
                return Created(nameof(GetAsync), messageDTO);
            }
            catch (HTMLMessageError ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BaseException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync([FromBody] HTMLMessageUpdateDTO messageDTO)
        {
            try { 
                await _messageService.UpdateMessageAsync(messageDTO, User.Identity.Name);
                return Ok();
            }
            catch (HTMLMessageError ex)
            {
                return BadRequest(ex.Message);
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
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try { 
                await _messageService.DeleteMessageAsync(id, User.Identity.Name);
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
        }
    }
}
