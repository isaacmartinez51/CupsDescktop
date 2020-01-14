using Repositories.Data.Entities;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IActionDetailRepository : IGenericRepository<ActionDetailEModel>
    {
        Task<List<ActionDetailVModel>> GetQueryActionDetailByIdUser(int id);
    }
}
