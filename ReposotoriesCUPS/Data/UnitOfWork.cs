using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Implementations;
using Repositories.Interfaces;

namespace ReposotoriesCUPS.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            UsersR = new UserRepository(_context);
            ReaderR = new ReaderRepository(_context);
            OrderR = new OrderRepository(_context);
            OrderDetailR = new OrderDetailRepository(_context);
        }

        public IUserRepository UsersR { get; private set; }
        public IReaderRepository ReaderR { get; private set; }
        public IOrderRepository OrderR { get; private set; }
        public IOrderDetailRepository OrderDetailR { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
