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
        public  IEnumerable<Message> GetAll()
        {
            return  _messageService.GetAllMessages();
        }

        // GET api/<MessageController>/5
        [HttpGet("{id}")]
        public async Task<Message> GetAsync(int id)
        {
            return await _messageService.GetMessageAsync(id);
        }

        // POST api/<MessageController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<MessageController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MessageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
