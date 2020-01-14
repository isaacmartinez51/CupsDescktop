using Repositories.Data;
using Repositories.Data.Entities;
using Repositories.Interfaces;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repositories.Implementations
{
    public class ReaderKindRepository : GenericRepository<ApplicationDbContext, ReaderKindEModel>, IReaderKindRepository
    {
        public bool Exist(Expression<Func<ReaderKindEModel, bool>> expression) => ReadsItems(expression).Count() > 0;
       

        public IQueryable<ReaderKindVModel> GetQueryReaderKind()
        {
            return ReadsItems().Select(x => new ReaderKindVModel
            {
                ReaderKindID = x.ReaderKindID,
                AntennaNumber = x.AntennaNumber,
                Brand = x.Brand,
                ModelName = x.ModelName
            });
        }
    }
}
