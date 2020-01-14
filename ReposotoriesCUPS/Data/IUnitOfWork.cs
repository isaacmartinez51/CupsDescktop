using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposotoriesCUPS.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UsersR { get; }

        IReaderRepository ReaderR { get; }

        IOrderRepository OrderR { get; }

        IOrderDetailRepository OrderDetailR { get; }

        int Complete();
    }
}
