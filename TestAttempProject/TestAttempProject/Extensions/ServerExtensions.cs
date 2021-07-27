using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TestAttemptProject.BLL.Interfaces;
using TestAttemptProject.BLL.Services;

using TestAttemptProject.DAL.Interfaces;
using TestAttemptProject.DAL.Realization;

using TestAttemptProject.Common.Entities;
using TestAttemptProject.Common.DTO;

using AutoMapper;

namespace TestAttemptProject.Extensions
{
    public static class ServerExtensions
    {
        public static void AddMyServices(this IServiceCollection service)
        {
            service.AddScoped<IMessageRepository, MessageRepository>();
            service.AddScoped<IHTMLMessageRepository, HTMLMessageRepository>();
            
            service.AddScoped<IMessageService, MessageService>();
            service.AddScoped<IHTMLMessageService, HTMLMessageService>();

            var mapperConfig = CreateMapperConfiguration();
            IMapper mapper = mapperConfig.CreateMapper();
            service.AddSingleton(mapper);
        }

        private static MapperConfiguration CreateMapperConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MessageCreateDTO, Message>()
                .ForMember(h => h.CreationStamp, opt => opt.MapFrom(dto => DateTime.Now));
                cfg.CreateMap<MessageUpdateDTO, Message>()
                .ForMember(h => h.EditTime, opt => opt.MapFrom(dto => DateTime.Now));
                cfg.CreateMap<HTMLMessageCreateDTO, HTMLMessage>()
                .ForMember(h => h.DataStamp, opt => opt.MapFrom(dto => DateTime.Now));
                cfg.CreateMap<HTMLMessageUpdateDTO, HTMLMessage>()
                .ForMember(h => h.EditDate, opt => opt.MapFrom(dto => DateTime.Now));
            });
            return config;
        }
    }
}
