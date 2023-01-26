using IEatPizzaSocialNetwork.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEatPizzaSocialNetwork.Infrastructure.Data.Context
{
    public class IEatPizzaSocialNetworkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Form> Forms { get; set; }

        public IEatPizzaSocialNetworkDbContext(DbContextOptions<IEatPizzaSocialNetworkDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
