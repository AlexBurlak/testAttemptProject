using System;
using Xunit;
using FakeItEasy;
using TestAttemptProject.DAL.Context;
using TestAttemptProject.Common.Entities;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using TestAttemptProject.DAL.Interfaces;
using TestAttemptProject.DAL.Realization;
using TestAttemptProject.Common.Exceptions;
using System.Threading.Tasks;

namespace TeastAttemptProject.Tests
{
    public class DAL_HTMLMessageRepository_UnitTests : IDisposable
    {
        private readonly UserDbContext _userDbContext;
        private readonly IHTMLMessageRepository _messageRepository;
        public void Dispose()
        {
            _userDbContext.Dispose();
        }
        private void Seed()
        {
            _userDbContext.Database.EnsureDeleted();
            _userDbContext.Database.EnsureCreated();
            var one = new HTMLMessage
            {
                Id = 1,
                Content = "<a>Hello</a> world",
                Author = new User(),
                DataStamp = new DateTime(2000, 01, 01)
            };
            var two = new HTMLMessage
            {
                Id = 2,
                Content = "<i>G</i>oodnight =)",
                Author = new User(),
                DataStamp = new DateTime(2001, 01, 01)
            };
            _userDbContext.HTMLMessages.AddRange(one, two);
            _userDbContext.SaveChanges();
        }
        public DAL_HTMLMessageRepository_UnitTests()
        {
            var userDbOptions = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: $"{nameof(DAL_HTMLMessageRepository_UnitTests)}.{Guid.NewGuid()}")
                .Options;
            _userDbContext = new UserDbContext(userDbOptions);

            Seed();

            _messageRepository = new HTMLMessageRepository(_userDbContext);

        }
        [Fact]
        public void GetAll_WhenDatabaseContainsTwoRecords_ThenGetEnumerableWithTwoRecords()
        {
            //Arange
            IEnumerable<HTMLMessage> messages;
            //Act
            messages = _messageRepository.GetAll();
            //Assert
            Assert.Equal(2, messages.Count());
        }

        [Fact]
        public void GetAsync_When1RecordExists_ThenGetRecordWithId1()
        {
            //Arange
            HTMLMessage message;
            //Act
            message = _messageRepository.GetAsync(1).Result;
            //Assert
            Assert.Equal(1, message.Id);
            Assert.Equal("<a>Hello</a> world", message.Content);
            Assert.Equal(new DateTime(2000, 01, 01), message.DataStamp);
        }

        [Fact]
        public async Task GetAsync_WhenRecordDoesntExists_ThenThrowException()
        {
            //Arange
            //Act
            //Assert
            await Assert.ThrowsAsync<MessageNotFoundException>(async () => await _messageRepository.GetAsync(100));
        }
        [Fact]
        public void UpdateAsync_WhenRecordIdExists_ThenUpdateRecord()
        {
            //Arange
            HTMLMessage message = new HTMLMessage { Id = 1, Content = "<b>I like</b> <i>pizza</i>"};
            //Act
            _messageRepository.UpdateAsync(message).Wait();
            var result = _messageRepository.GetAsync(message.Id).Result;
            //Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("<b>I like</b> <i>pizza</i>", result.Content);
        }
        [Fact]
        public async Task UpdateAsync_WhenRecordIdDoesntExists_ThenGetException()
        {
            //Arange
            HTMLMessage message = new HTMLMessage { Id = 100, Content = "<b>I like</b> <i>pizza</i>"};
            //Act - Assert
            await Assert.ThrowsAsync<MessageNotFoundException>(async () => await _messageRepository.UpdateAsync(message));
        }
        [Fact]
        public void DeleteAsync_WhenRecordIdExists_ThenRecordDeletes()
        {
            _userDbContext.Database.EnsureCreated();
            //Arange
            int messageId = 1;
            //Act
            _messageRepository.DeleteAsync(messageId).Wait();
            var messages = _messageRepository.GetAll();
            //Assert
            Assert.Null(messages.FirstOrDefault(m => m.Id == messageId));
        }
        [Fact]
        public async Task DeleteAsync_WhenRecordIdDoesntExists_ThenThrownError()
        {
            //Arange
            int messageId = 101;
            //Act - Assert
            await Assert.ThrowsAsync<MessageNotFoundException>(async () => await _messageRepository.DeleteAsync(messageId));
        }
        [Fact]
        public void AddAsync_WhenRecordIsUntracked_ThenRecordCreated()
        {
            //Arange
            HTMLMessage message = new HTMLMessage { Content = "<h3>Ok(</h3>", Author = new User() };
            //Act 
            _messageRepository.AddAsync(message).Wait();
            var result = _messageRepository.GetAll().FirstOrDefault(msg => msg.Id == 3);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("<h3>Ok(</h3>", result.Content);
        }
    }
}
