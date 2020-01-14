using Repositories.Data;
using Repositories.Data.Entities;
using Repositories.Interfaces;
using Repositories.ViewModels;
using ReposotoriesCUPS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repositories.Implementations
{
    public class UserRepository : GenericRepository<UserEModel>, IUserRepository
    {

        public UserRepository(ApplicationDbContext context): base (context)
        {

        }

        public bool Exist(Expression<Func<UserEModel, bool>> expression) => ReadsItems(expression).Count() > 0;


        public IQueryable<UserVModel> GetQueryAllUser()
        {
            return ReadsItems().Select(x => new UserVModel
            {
                UserID = x.UserID,
                FirtsName = x.FirtsName,
                LastName = x.LastName,
                UserName = x.UserName
            });
        }

        //public UserVModel GetQueryByUserId(int userId)
        //{
        //    var user = GetItemByExpression(x => x.UserID == userId);
        //    UserVModel userV = new UserVModel
        //    {
        //        UserID = user.UserID,
        //        FirtsName = user.FirtsName,
        //        LastName = user.LastName,
        //        UserName = user.UserName,
        //        Password = user.Password,
        //        IsSuperAdmin = user.IsSuperAdmin,
        //        Status = user.Status
        //    };
        //    return userV;
        //}

        //public UserVModel GetQueryByUserName(string userName)
        //{
        //    var user =  GetItemByExpression(x => x.UserName == userName);
        //    UserVModel userV = new UserVModel
        //    {
        //        UserID = user.UserID,
        //        UserName = user.UserName,
        //        Password = user.Password,
        //        IsSuperAdmin = user.IsSuperAdmin,
        //        Status = user.Status
        //    };
        //    return userV;
        //}
    }
}
