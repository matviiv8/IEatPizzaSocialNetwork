using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IEatPizzaSocialNetwork.Domain.Core.Entities;

namespace IEatPizzaSocialNetwork.Domain.Interfaces.Interfaces
{
    public interface IFormRepository : IDisposable
    {
        IEnumerable<Form> GetFormList();

        Form GetForm(int id);

        Form GetFormByUserId(int userId);

        void Create(Form form);

        void Update(Form form);

        void Save();
    }
}
