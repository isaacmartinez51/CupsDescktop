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
    public class OrderDetailRepository : GenericRepository<OrderDetailEModel>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {

        }
        public bool Exist(Expression<Func<OrderDetailEModel, bool>> expression) => ReadsItems(expression).Count() > 0;


        public IQueryable<OrderDetailVModel> GetQueryOrderDetail()
        {
            return ReadsItems().Select(x => new OrderDetailVModel
            {
                OrderID = x.OrderID,
                OrderDetailID = x.OrderDetailID,
                embarque = x.embarque,
                partida = x.partida,
                total_pallets = x.total_pallets,
                continentalpartnumber = x.continentalpartnumber,
                customerpartnumber = x.customerpartnumber,
                cantidad = x.cantidad,
                delivery = x.delivery,
                traza = x.traza,
                shipment = x.shipment,
                notas = x.notas,
                Leido = x.Leido

            });
        }

        public IQueryable<OrderDetailVModel> GetQueryOrderDetail(int id)
        {
            var order = ApplicationDbContext.Order.FirstOrDefault(x => x.OrderID == id);
            var portal = ApplicationDbContext.Reader.FirstOrDefault(x => x.ReaderID == order.ReaderID);
            return ReadsItems(x => x.OrderID == id).Select(x => new OrderDetailVModel
            {
                OrderID = x.OrderID,
                OrderDetailID = x.OrderDetailID,
                embarque = x.embarque,
                partida = x.partida,
                total_pallets = x.total_pallets,
                continentalpartnumber = x.continentalpartnumber,
                customerpartnumber = x.customerpartnumber,
                cantidad = x.cantidad,
                delivery = x.delivery,
                traza = x.traza,
                shipment = x.shipment,
                notas = x.notas,
                Leido = x.Leido,
                Portal = portal.Name
            });
        }
        public OrderDetailEModel GetQueryOrderDetail(int orderId, string partNumber)
        {
            OrderDetailEModel model = ReadsItems(x => x.OrderID == orderId && x.continentalpartnumber == partNumber && x.Leido == false).FirstOrDefault();
            return model;
            //return 
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
