using IEatPizzaSocialNetwork.Domain.Core.Entities;
using IEatPizzaSocialNetwork.Domain.Interfaces.Interfaces;
using IEatPizzaSocialNetwork.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEatPizzaSocialNetwork.Infrastructure.Data.Repository
{
    public class FormRepository : IFormRepository
    {
        private IEatPizzaSocialNetworkDbContext _context;
        private bool disposed = false;

        public FormRepository(IEatPizzaSocialNetworkDbContext context)
        {
            this._context = context;
        }

        public void Create(Form form)
        {
            _context.Forms.Add(form);
        }

        public void Update(Form form)
        {
            _context.Entry(form).State = EntityState.Modified;
        }

        public Form GetForm(int id)
        {
            return _context.Forms.Find(id);
        }

        public IEnumerable<Form> GetFormList()
        {
            return _context.Forms.ToList();
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

        public Form GetFormByUserId(int userId)
        {
            return _context.Forms.Where(form => form.UserId == userId).FirstOrDefault();
        }
    }
}
