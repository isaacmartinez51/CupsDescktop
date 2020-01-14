using Microsoft.EntityFrameworkCore;
using Repositories.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposotoriesCUPS.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(){}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // LocalConnectionCUP   ConnectionCUP
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["LocalConnectionCup"].ConnectionString).EnableSensitiveDataLogging(true);
        }

        #region DbSet

        public DbSet<UserEModel> User { get; set; } 
        public DbSet<OrderEModel> Order { get; set; }
        public DbSet<ReaderEModel> Reader { get; set; }
        public DbSet<OrderDetailEModel> OrderDetail { get; set; }
        public DbSet<ReaderKindEModel> ReaderKind { get; set; }
        public DbSet<ActionDetailEModel> ActionDetail { get; set; }
        public DbSet<ActionEModel> Action { get; set; }
        public DbSet<RoleActionEModel> RoleAction { get; set; }
        public DbSet<RoleEModel> Role { get; set; }
        public DbSet<UserRoleEModel> UserRole { get; set; }
        public DbSet<LocationEModel> Location { get; set; }

        #endregion
    }
}
