using IEatPizzaSocialNetwork.Domain.Core.Entities;
using IEatPizzaSocialNetwork.Domain.Interfaces.Interfaces;
using IEatPizzaSocialNetwork.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEatPizzaSocialNetwork.Infrastructure.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private IEatPizzaSocialNetworkDbContext _context;
        private bool disposed = false;
        
        public UserRepository(IEatPizzaSocialNetworkDbContext context)
        {
            this._context = context;
        }

        public void Create(User user)
        {
            _context.Users.Add(user);
        }

        public IEnumerable<User> GetUserList()
        {
            return _context.Users.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
