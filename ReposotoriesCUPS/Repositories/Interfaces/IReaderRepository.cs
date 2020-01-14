using Repositories.Data.Entities;
using Repositories.ViewModels;
using ReposotoriesCUPS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repositories.Interfaces
{
    public interface IReaderRepository : IGenericRepository<ReaderEModel>
    {
        /// <summary>
        /// Validate if exist
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool Exist(Expression<Func<ReaderEModel, bool>> expression);

        /// <summary>
        /// Entity model to view model
        /// </summary>
        /// <returns></returns>
        IQueryable<ReaderVModel> GetQueryReader();
    }
}
