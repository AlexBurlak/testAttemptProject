using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAttemptProject.BLL.Interfaces;
using TestAttemptProject.Domain.Entities;
using TestAttemptProject.Domain.DTO;

namespace TestAttemptProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        // GET: api/<MessageController>
        [HttpGet]
        public  ActionResult<IEnumerable<Message>> GetAll()
        {
            return  Ok(_messageService.GetAllMessages());
        }

        // GET api/<MessageController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetAsync(int id)
        {
            return Ok(await _messageService.GetMessageAsync(id));
        }

        // POST api/<MessageController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MessageCreateDTO messageDTO)
        {
            await _messageService.AddMessageToDbAsync(messageDTO);
            return Created(nameof(GetAsync), messageDTO);
        }

        // PUT api/<MessageController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] MessageUpdateDTO messageDTO)
        {
            _messageService.UpdateMessage(messageDTO);
        }

        // DELETE api/<MessageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
