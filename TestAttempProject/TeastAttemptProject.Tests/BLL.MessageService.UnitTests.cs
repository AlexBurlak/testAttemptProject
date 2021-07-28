using System;
using Xunit;
using FakeItEasy;
using TestAttemptProject.DAL.Context;
using TestAttemptProject.Common.Entities;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using TestAttemptProject.DAL.Interfaces;
using TestAttemptProject.DAL.Realization;
using TestAttemptProject.Common.Exceptions;
using System.Threading.Tasks;
using TestAttemptProject.BLL.Interfaces;
using TestAttemptProject.BLL.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TestAttemptProject.Common.DTO;

namespace TeastAttemptProject.Tests
{
    public class BLL_MessageService_UnitTests
    {
        private readonly UserDbContext _userDbContext;
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        public void Dispose()
        {
            _userDbContext.Dispose();
        }
        private List<User> Users;
        private Dictionary<User, string> Roles;
        private void Seed()
        {
            

            Users = new List<User>
            {
                new User() {UserName = "user"},
                new User() {UserName = "admin"}
            };

            Roles = new Dictionary<User, string>
            {
                {Users.First(), "User"},
                {Users.Last(), "Admin" }
            };

            _userDbContext.Database.EnsureDeleted();
            _userDbContext.Database.EnsureCreated();
            var one = new Message
            {
                Id = 1,
                Content = "Hello world",
                Author = Users.First(),
                CreationStamp = new DateTime(2000, 01, 01)
            };
            var two = new Message
            {
                Id = 2,
                Content = "Goodnight =)",
                Author = Users.Last(),
                CreationStamp = new DateTime(2001, 01, 01)
            };
            _userDbContext.Messages.AddRange(one, two);
            _userDbContext.SaveChanges();
        }
        public BLL_MessageService_UnitTests()
        {
            var userDbOptions = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: $"{nameof(BLL_MessageService_UnitTests)}.{Guid.NewGuid()}")
                .Options;
            _userDbContext = new UserDbContext(userDbOptions);

            Seed();

            var mapperConfig = CreateMapperConfiguration();
            IMapper _mapper = mapperConfig.CreateMapper();

            var fakeUserManager = A.Fake<UserManager<User>>();

            A.CallTo(() => fakeUserManager.FindByNameAsync(A<string>.Ignored))
            .ReturnsLazily((string message) =>
            {
                return Task.FromResult(Users.Find(u => u.UserName == message));
            });
            A.CallTo(() => fakeUserManager.IsInRoleAsync(A<User>.Ignored,A<string>.Ignored))
            .ReturnsLazily((User user, string role) =>
            {
                return Task.FromResult(Roles[user] == role);
            });
            _messageRepository = new MessageRepository(_userDbContext);
            _messageService = new MessageService(_mapper,_messageRepository, fakeUserManager);
        }
        private static MapperConfiguration CreateMapperConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MessageCreateDTO, Message>()
                .ForMember(h => h.CreationStamp, opt => opt.MapFrom(dto => DateTime.Now));
                cfg.CreateMap<MessageUpdateDTO, Message>()
                .ForMember(h => h.EditTime, opt => opt.MapFrom(dto => DateTime.Now));
            });
            return config;
        }
        [Fact]
        public async Task AddMessageToDbAsync_WhenMessageContentNotNull_ThenMessageAdded()
        {
            //Arrange
            var messageDto = new MessageCreateDTO() { Content = "Content" };
            string userName = "user";
            //Act
            await _messageService.AddMessageToDbAsync(messageDto, userName);
            var result = _userDbContext.Messages.FirstOrDefault(m => m.Id == 3);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("Content", result.Content);
            Assert.Equal(Users.First(), result.Author);
            Assert.Equal(3, result.Id);
        }
        [Fact]
        public async Task AddMessageToDbAsync_WhenMessageContentNull_ThenThrowException()
        {
            //Arrange
            var messageDto = new MessageCreateDTO() { Content = null };
            string userName = "user";
            //Act 
            //Assert
            await Assert.ThrowsAsync<BaseException>(async () => await _messageService.AddMessageToDbAsync(messageDto, userName));
        }
        [Fact]
        public async Task GetAllMessages_WhenUserIsUserAndListContain2Elements_ThenGetOneMessage()
        {
            //Arrange
            string userName = "user";
            //Act 
            var result = await _messageService.GetAllMessages(userName);
            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count());
        }
        [Fact]
        public async Task GetAllMessages_WhenUserIsAdminAndListContain2Elements_ThenGetAllMessages()
        {
            //Arrange
            string userName = "admin";
            //Act 
            var result = await _messageService.GetAllMessages(userName);
            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }
        [Fact]
        public async Task UpdateMessageAsync_WhenAccessAllowedAndContentNotNull_ThenUpdateMessage()
        {
            //Arrange
            string userName = "user";
            MessageUpdateDTO message = new MessageUpdateDTO() { Id = 1, Content = "New Content"};
            //Act 
            await _messageService.UpdateMessageAsync(message,userName);
            var result = await _messageService.GetMessageAsync(message.Id, userName);
            //Assert
            Assert.NotNull(result);
            Assert.Equal("New Content", result.Content);
        }
        [Fact]
        public async Task UpdateMessageAsync_WhenAccessNotAllowedAndContentNotNull_ThenThrowAccesForbidenEception()
        {
            //Arrange
            string userName = "user";
            MessageUpdateDTO message = new MessageUpdateDTO() { Id = 2, Content = "New Content"};
            //Act  //Assert
            await Assert.ThrowsAsync<AccesForbidenException>(async () => await _messageService.UpdateMessageAsync(message,userName));
        }
        [Fact]
        public async Task UpdateMessageAsync_WhenAccessAllowedAndContentNull_ThenThrowBaseException()
        {
            //Arrange
            string userName = "user";
            MessageUpdateDTO message = new MessageUpdateDTO() { Id = 1, Content = null};
            //Act  //Assert
            await Assert.ThrowsAsync<BaseException>(async () => await _messageService.UpdateMessageAsync(message,userName));
        }
        [Fact]
        public async Task DeleteMessageAsync_WhenAccessAllowed_ThenMessageDeletes()
        {
            //Arrange
            string userName = "user";
            int messageId = 1;
            //Act  
            await _messageService.DeleteMessageAsync(messageId, userName);
            var result = _userDbContext.Messages.FirstOrDefault(m => m.Id == messageId);
            //Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task DeleteMessageAsync_WhenAccessNotAllow_ThenThrowAccessForbidenException()
        {
            //Arrange
            string userName = "user";
            int messageId = 2;
            //Act  //Assert
            await Assert.ThrowsAsync<AccesForbidenException>(async () => await _messageService.DeleteMessageAsync(messageId,userName));
        }
        [Fact]
        public async Task DeleteMessageAsync_WhenUserIsAdmin_ThenMessageDeletes()
        {
            //Arrange
            string userName = "admin";
            int messageId = 2;
            //Act  
            await _messageService.DeleteMessageAsync(messageId, userName);
            var result = _userDbContext.Messages.FirstOrDefault(m => m.Id == messageId);
            //Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetMessageAsync_WhenAccessAllowed_ThenReturnsMessage()
        {
            //Arrange
            string userName = "user";
            int messageId = 1;
            //Act  
            var result = await _messageService.GetMessageAsync(messageId, userName);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetMessageAsync_WhenAccessNotAllowed_ThenThrowsAccessForbidenException()
        {
            //Arrange
            string userName = "user";
            int messageId = 2;
            //Act  //Assert
            await Assert.ThrowsAsync<AccesForbidenException>(async () => await _messageService.GetMessageAsync(messageId,userName));
        }
        [Fact]
        public async Task GetMessageAsync_WhenUserIsAdmin_ThenReturnsMessage()
        {
            //Arrange
            string userName = "admin";
            int messageId = 1;
            //Act  
            var result = await _messageService.GetMessageAsync(messageId, userName);
            //Assert
            Assert.NotNull(result);
        }
    }
}
