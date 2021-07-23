using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestAttemptProject.Domain.Entities;

namespace TestAttemptProject.DAL.Context
{
    public class TestAttemptDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        
        public TestAttemptDbContext(DbContextOptions<TestAttemptDbContext> options)
            : base(options)
        { }
    }
}
