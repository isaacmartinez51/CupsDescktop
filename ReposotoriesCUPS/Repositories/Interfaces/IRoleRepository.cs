using Repositories.Data.Entities;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRoleRepository : IGenericRepository<RoleEModel>
    {
        Task<List<RoleVModel>> GetQueryRoleByUserId(int id);
    }
}
