using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class 
    {
        protected readonly DbContext Context;
       
        /// <summary>
        /// ctro
        /// </summary>
        /// <param name="options"></param>
        public GenericRepository(DbContext context)
        {
            Context = context;
        }
        /// <summary>
        /// Create New Item
        /// </summary>
        /// <param name="item"></param>
        public virtual TEntity CreateItem(TEntity item)
        {
            // Initialize transaction
            
                // Get Primary Key from entity
                string keyPropertyName = GetPrimaryKey();

                Type keyPropertyType = typeof(TEntity)
                    .GetProperty(keyPropertyName)
                    .PropertyType;
                // Generate consecutive identity if pk is number 
                // else if pf is guid type generate new one
                if (typeof(int).Equals(keyPropertyType))
                    item.GetType().GetProperty(keyPropertyName)
                    .SetValue(item, GetCurrentId(keyPropertyName));

                else if (typeof(Guid).Equals(keyPropertyType))
                    item.GetType().GetProperty(keyPropertyName)
                    .SetValue(item, Guid.NewGuid());
                // Add item into entity
                Context.Set<TEntity>().Add(item);
                // Save changes and commit transaction
                Context.SaveChanges();
                
            
            return item;
        }
        /// <summary>
        /// Create item async
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> CreateItemAsync(TEntity item)
        {
            // Initialize transaction
            using (var transaction = await Context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead))
            {
                // Get Primary Key from entity
                string keyPropertyName = GetPrimaryKey();

                Type keyPropertyType = typeof(TEntity)
                    .GetProperty(keyPropertyName)
                    .PropertyType;
                // Generate consecutive identity if pk is number 
                // else if pf is guid type generate new one
                if (typeof(int).Equals(keyPropertyType))
                    item.GetType().GetProperty(keyPropertyName)
                    .SetValue(item, GetCurrentId(keyPropertyName));

                else if (typeof(Guid).Equals(keyPropertyType))
                    item.GetType().GetProperty(keyPropertyName)
                    .SetValue(item, Guid.NewGuid());
                // Add item into entity
                Context.Set<TEntity>().Add(item);
                // Save changes and commit transaction
                await Context.SaveChangesAsync();
                transaction.Commit();
            }
            return item;
        }
        /// <summary>
        /// Create new item with composed key 
        /// </summary>
        /// <param name="item"></param>
        public virtual TEntity CreateItemComposedKey(TEntity item)
        {
            // Initialize transaction
            using (var transaction = Context.Database.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                // Add item into entity
                Context.Set<TEntity>().Add(item);
                // Save changes and commit transaction
                Context.SaveChanges();
                transaction.Commit();
            }
            return item;
        }
        /// <summary>
        /// Create new item with composed key async
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task<TEntity> CreateItemComposedKeyAsync(TEntity item)
        {
             //Initialize transaction
            using (var transaction = await Context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead))
            {
                 //Add item into entity
                Context.Set<TEntity>().Add(item);
                 //Save changes and commit transaction
                await Context.SaveChangesAsync();
                transaction.Commit();
            }
            return item;
        }
        /// <summary> Get item by expression </summary>
        /// <returns></returns>
        // public TEntity GetItemByExpression(Expression<Func<TEntity, bool>> expression) => Context.Set<TEntity>().FirstOrDefault(expression);
        /// <summary>
        /// Reads all items
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> ReadsItems() => ReadsItems(all => true);
        /// <summary>
        /// Reads items by expreInvon
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IQueryable<TEntity> ReadsItems(Expression<Func<TEntity, bool>> expression)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>().Where(expression);
            return query;
        }

        #region Sin autoincrementable
        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item"></param>
        public TEntity UpdateItem(TEntity item)
        {
            // Get Primary Key from entity
            string keyPropertyName = GetPrimaryKey();
            using (var transaction = Context.Database
                .BeginTransaction(IsolationLevel.RepeatableRead))
            {
                TEntity original = Context.Set<TEntity>()
                    .Where(x => x.GetType()
                       .GetProperty(keyPropertyName).GetValue(x, null)
                       .Equals(item.GetType().GetProperty(keyPropertyName)
                           .GetValue(item, null)))
                    .FirstOrDefault();
                var originalEntry = Context.Entry(original);
                foreach (var property in originalEntry.Metadata.GetProperties())
                {
                    //Set current value and original value to verify if not are equals
                    // if not are equals set original value from current value                                                 
                    var proposedValue = item.GetType()
                        .GetProperty(property.Name).GetValue(item, null);
                    var originalValue = originalEntry
                        .Property(property.Name).OriginalValue;
                    if (proposedValue != null)
                        if (!proposedValue.Equals(originalValue))
                            original.GetType().GetProperty(property.Name)
                            .SetValue(original, proposedValue);
                }
                // Save changes and commit transaction
                Context.SaveChanges();
                transaction.Commit();
            }
            return item;
        }


        /// <summary>
        /// Update item Async
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> UpdateItemAsync(TEntity item)
        {
            // Get Primary Key from entity
            string keyPropertyName = GetPrimaryKey();
            using (var transaction = await Context.Database
                .BeginTransactionAsync(IsolationLevel.RepeatableRead))
            {
                TEntity original = await Context.Set<TEntity>()
                    .Where(x => x.GetType()
                       .GetProperty(keyPropertyName).GetValue(x, null)
                       .Equals(item.GetType().GetProperty(keyPropertyName)
                           .GetValue(item, null)))
                    .FirstOrDefaultAsync();
                var originalEntry = Context.Entry(original);
                foreach (var property in originalEntry.Metadata.GetProperties())
                {
                    //Set current value and original value to verify if not are equals
                    // if not are equals set original value from current value                                                 
                    var proposedValue = item.GetType()
                        .GetProperty(property.Name).GetValue(item, null);
                    var originalValue = originalEntry
                        .Property(property.Name).OriginalValue;
                    if (proposedValue != null)
                        if (!proposedValue.Equals(originalValue))
                            original.GetType().GetProperty(property.Name)
                            .SetValue(original, proposedValue);
                }
                // Save changes and commit transaction
                await Context.SaveChangesAsync();
                transaction.Commit();
            }
            return item;
        }
        #endregion
        /// <summary>
        /// Update item with composed key 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual TEntity UpdateItemComposedKey(Expression<Func<TEntity, bool>> expression, TEntity item)
        {
            using (var transaction = Context.Database
                .BeginTransaction(IsolationLevel.RepeatableRead))
            {
                TEntity original = Context.Set<TEntity>().Where(expression).FirstOrDefault();
                var originalEntry = Context.Entry(original);
                foreach (var property in originalEntry.Metadata.GetProperties())
                {
                    //Set current value and original value to verify if not are equals
                    // if not are equals set original value from current value                                                 
                    var proposedValue = item.GetType()
                        .GetProperty(property.Name).GetValue(item, null);
                    var originalValue = originalEntry
                        .Property(property.Name).OriginalValue;
                    if (proposedValue != null)
                        if (!proposedValue.Equals(originalValue))
                            original.GetType().GetProperty(property.Name)
                            .SetValue(original, proposedValue);
                }
                // Save changes and commit transaction
                Context.SaveChanges();
                transaction.Commit();
            }
            return item;
        }
        /// <summary>
        /// Update item with composed key async
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> UpdateItemComposedKeyAsync(Expression<Func<TEntity, bool>> expression, TEntity item)
        {
            using (var transaction = await Context.Database
                .BeginTransactionAsync(IsolationLevel.RepeatableRead))
            {
                TEntity original = await Context.Set<TEntity>().Where(expression).FirstOrDefaultAsync();
                var originalEntry = Context.Entry(original);
                foreach (var property in originalEntry.Metadata.GetProperties())
                {
                    //Set current value and original value to verify if not are equals
                    // if not are equals set original value from current value                                                 
                    var proposedValue = item.GetType()
                        .GetProperty(property.Name).GetValue(item, null);
                    var originalValue = originalEntry
                        .Property(property.Name).OriginalValue;
                    if (proposedValue != null)
                        if (!proposedValue.Equals(originalValue))
                            original.GetType().GetProperty(property.Name)
                            .SetValue(original, proposedValue);
                }
                // Save changes and commit transaction
                await Context.SaveChangesAsync();
                transaction.Commit();
            }
            return item;
        }

        /// <summary>
        /// Get current id for primary key
        /// </summary>
        /// <param name="keyPropertyName"></param>
        /// <returns></returns>
        private int GetCurrentId(string keyPropertyName)
        {
            var lastItem = Context.Set<TEntity>().LastOrDefault();
            int id = lastItem != null ?
                (int)lastItem.GetType()
                .GetProperty(keyPropertyName)
                .GetValue(lastItem, null) : 0;
            return id += 1;
        }
        /// <summary>
        /// Get property name that have KeyAttribute
        /// </summary>
        /// <returns></returns>
        private string GetPrimaryKey()
        {
            return typeof(TEntity).GetProperties()
                .Where(
                    x => x.CustomAttributes
                    .Where(y => y.AttributeType.Name == "KeyAttribute")
                    .Count() > 0
                ).FirstOrDefault().Name;
        }
    }
}
