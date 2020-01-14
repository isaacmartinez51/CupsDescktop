using Repositories.Data;
using Repositories.Data.Entities;
using Repositories.Interfaces;
using Repositories.ViewModels;
using ReposotoriesCUPS.Data;
using ReposotoriesCUPS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repositories.Implementations
{
    public class ReaderRepository : GenericRepository<ReaderEModel>, IReaderRepository
    {
        public ReaderRepository(ApplicationDbContext context) : base(context)
        {

        }
        public bool Exist(Expression<Func<ReaderEModel, bool>> expression) => ReadsItems(expression).Count() > 0;
       

        public IQueryable<ReaderVModel> GetQueryReader()
        {
            return ReadsItems().Select(x => new ReaderVModel
            {
                ReaderID = x.ReaderID,
                ReaderKindID = x.ReaderKindID,
                Name = x.Name,
                IpAddress = x.IpAddress
            });
        }
    }
}
