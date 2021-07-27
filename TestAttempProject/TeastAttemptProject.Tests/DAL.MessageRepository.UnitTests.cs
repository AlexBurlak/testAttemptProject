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
    public class DAL_MessageRepository_UnitTests : IDisposable
    {
        private readonly UserDbContext _userDbContext;
        private readonly IMessageRepository _messageRepository;
        public void Dispose()
        {
            _userDbContext.Dispose();
        }
        private void Seed()
        {
            _userDbContext.Database.EnsureDeleted();
            _userDbContext.Database.EnsureCreated();
            var one = new Message
            {
                Id = 1,
                Content = "Hello world",
                Author = new User(),
                CreationStamp = new DateTime(2000, 01, 01)
            };
            var two = new Message
            {
                Id = 2,
                Content = "Goodnight =)",
                Author = new User(),
                CreationStamp = new DateTime(2001, 01, 01)
            };
            _userDbContext.Messages.AddRange(one, two);
            _userDbContext.SaveChanges();
        }
        public DAL_MessageRepository_UnitTests()
        {
            var userDbOptions = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _userDbContext = new UserDbContext(userDbOptions);
           
            Seed();

            _messageRepository = new MessageRepository(_userDbContext);
           
        }
        [Fact]
        public void GetAll_WhenDatabaseContainsTwoRecords_ThenGetEnumerableWithTwoRecords()
        {
            //Arange
            IEnumerable<Message> messages;
            //Act
            messages = _messageRepository.GetAll();
            //Assert
            Assert.Equal(2, messages.Count());
        }

        [Fact]
        public void GetAsync_When1RecordExists_ThenGetRecordWithId1()
        {
            //Arange
            Message message;
            //Act
            message = _messageRepository.GetAsync(1).Result;
            //Assert
            Assert.Equal(1, message.Id);
            Assert.Equal("Hello world", message.Content);
            Assert.Equal(new DateTime(2000, 01, 01), message.CreationStamp);
        }
        
        [Fact]
        public void GetAsync_WhenRecordDoesntExists_ThenGetNull()
        {
            //Arange
            Message message;
            //Act
            message = _messageRepository.GetAsync(100).Result;
            //Assert
            Assert.Null(message);
        }
        [Fact]
        public void UpdateAsync_WhenRecordIdExists_ThenUpdateRecord()
        {
            //Arange
            Message message = new Message { Id = 1, Content = "I like pizza", Author = new User() };
            //Act
            _messageRepository.UpdateAsync(message).Wait();
            var result = _messageRepository.GetAsync(message.Id).Result;
            //Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("I like pizza", result.Content);
        }
        [Fact]
        public async Task UpdateAsync_WhenRecordIdDoesntExists_ThenGetException()
        {
            //Arange
            Message message = new Message { Id = 100, Content = "I like pizza", Author = new User()};
            //Act - Assert
            await Assert.ThrowsAsync<BaseException>(async () => await _messageRepository.UpdateAsync(message));
        }
        [Fact]
        public void DeleteAsync_WhenRecordIdExists_ThenRecordDeletes()
        {
            //Arange
            int messageId = 1;
            //Act
            _messageRepository.DeleteAsync(messageId).Wait();
            var message = _messageRepository.GetAsync(messageId).Result;
            //Assert
            Assert.Null(message);
        }
        [Fact]
        public async Task DeleteAsync_WhenRecordIdDoesntExists_ThenThrownError()
        {
            //Arange
            int messageId = 101;
            //Act - Assert
            await Assert.ThrowsAsync<BaseException>(async () => await _messageRepository.DeleteAsync(messageId));
        }
        [Fact]
        public void AddAsync_WhenRecordIsUntracked_ThenRecordCreated()
        {
            //Arange
            Message message = new Message { Content = "Ok(", Author = new User() };
            //Act 
            _messageRepository.AddAsync(message).Wait();
            var result = _messageRepository.GetAll().FirstOrDefault(msg => msg.Content == "Ok(");
            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
            Assert.Equal(new DateTime(2000, 01, 01), result.CreationStamp);
        }

    }
}
