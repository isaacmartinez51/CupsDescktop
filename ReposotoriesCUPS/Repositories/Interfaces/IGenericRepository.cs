using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IGenericRepository <TModel> where TModel : class
    {
        /// <summary> Get item by expression </summary>
        /// <returns></returns>
        // TModel GetItemByExpression(Expression<Func<TModel, bool>> expression);
        /// <summary>
        /// Reads all items
        /// </summary>
        /// <returns></returns>
        IQueryable<TModel> ReadsItems();
        /// <summary>
        /// Reads items by expreInvon
        /// </summary>
        /// <param name="expression">ExpreInvon to evaluate</param>
        /// <returns></returns>
        IQueryable<TModel> ReadsItems(Expression<Func<TModel, bool>> expression);
        /// <summary>
        /// Create new item
        /// </summary>
        /// <param name="item"></param>
        TModel CreateItem(TModel item);
        /// <summary>
        /// Create new item async
        /// </summary>
        /// <param name="item"></param>
        Task<TModel> CreateItemAsync(TModel item);
        /// <summary>
        /// Create new item with composed key async
        /// </summary>
        /// <param name="item"></param>
        Task<TModel> CreateItemComposedKeyAsync(TModel item);
        /// <summary>
        /// Create new item with composed key 
        /// </summary>
        /// <param name="item"></param>
        TModel CreateItemComposedKey(TModel item);
        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item"></param>
        TModel UpdateItem(TModel item);
        /// <summary>
        /// Update item async
        /// </summary>
        /// <param name="item"></param>
        Task<TModel> UpdateItemAsync(TModel item);
        /// <summary>
        /// Update item with composed key 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        TModel UpdateItemComposedKey(Expression<Func<TModel, bool>> expression, TModel item);
        /// <summary>
        /// Update item with composed key async
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<TModel> UpdateItemComposedKeyAsync(Expression<Func<TModel, bool>> expression, TModel item);

    }
}
