using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IEatPizzaSocialNetwork.Domain.Core.Entities;

namespace IEatPizzaSocialNetwork.Domain.Interfaces.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<User> GetUserList();

        void Create(User user);

        void Save();
    }
}
