using Repositories.Data;
using Repositories.Data.Entities;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ReposotoriesCUPS.Data;

namespace Repositories.Implementations
{
    public class OrderRepository : GenericRepository<OrderEModel>, IOrderRepository
    {
        // private readonly ApplicationDbContext Context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
           
        }
     

        #region New
        public bool Exist(Expression<Func<OrderEModel, bool>> expression) => ReadsItems(expression).Count() > 0;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="embarque"></param>
        /// <returns></returns>
        public OrderVModel GetOrderOnshipment(string embarque)
        {
            
                OrderVModel order = (from orders in ReadsItems(x => x.ShipmentNumber == embarque && x.OnShipment == true)
                                     join reader in ApplicationDbContext.Reader on orders.ReaderID equals reader.ReaderID
                                     select new
                                     {
                                         orders,
                                         reader
                                     }).Select(x => new OrderVModel
                                     {
                                         OrderID = x.orders.OrderID,
                                         ReaderID = x.orders.ReaderID,
                                         Number = x.orders.Number,
                                         ShipmentNumber = x.orders.ShipmentNumber,
                                         OnShipment = x.orders.OnShipment,
                                         Date = x.orders.Date,
                                         Finished = x.orders.Finished,
                                     }).FirstOrDefault();
              
                return order;
            
           
        }

        public OrderVModel GetQueryOrderComplete(string shipment)
        {

            OrderVModel order = new OrderVModel();
                List<OrderDetailVModel> query = ApplicationDbContext.OrderDetail
                       .Where(x => x.embarque == shipment).Select(x => new OrderDetailVModel
                       {
                           OrderID = x.OrderID,
                           partida = x.partida,
                           total_pallets = x.total_pallets,
                           continentalpartnumber = x.continentalpartnumber,
                           customerpartnumber = x.customerpartnumber,
                           traza = x.traza,
                           notas = x.notas,
                           Leido = x.Leido
                       }).ToList();

                //OrderVModel order = ReadsItems(x => x.ShipmentNumber == shipment).Select(x => new OrderVModel
                //{
                //    OrderID = x.OrderID,
                //    ReaderID = x.ReaderID,
                //    Number = x.Number,
                //    ShipmentNumber = x.ShipmentNumber,
                //    OnShipment = x.OnShipment,
                //    Date = x.Date,
                //    Finished = x.Finished
                //}).FirstOrDefault();

                order.ListOrderDetail = query;
                return order;


            
        }

        #endregion


        #region CreateShipment_V2
        /// <summary>
        /// Metodo para crear un embarque ysus detalles
        /// </summary>
        /// <param name="orderVModel"></param>
        /// <param name="orderDetailVModel"></param>
        /// <returns></returns>
        public async Task<OrderVModel> CreateShipmentV2(List<OrderDetailVModel> orderDetailVModel)
        {
            try
            {
                var todo = ApplicationDbContext.Order;
                int pallet = 1;
                OrderEModel orderModel = new OrderEModel();
                OrderVModel order = new OrderVModel();
                OrderDetailEModel orderDetail = new OrderDetailEModel();
                List<OrderDetailVModel> orderDetailList = new List<OrderDetailVModel>();

                using (var transaction = await ApplicationDbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead))
                {
                    // var todo = ApplicationDbContext.Order.ToList();
                    var lastItemOrder = ApplicationDbContext.Order.LastOrDefault();
                    int idOrder = lastItemOrder == null ? 1 : lastItemOrder.OrderID + 1;
                    // Crea el modelo para insertar en la tabla Order
                    orderModel = new OrderEModel()
                    {
                        OrderID = idOrder,
                        Date = DateTime.Now,
                        OnShipment = false,
                        ReaderID = null,
                        ShipmentNumber = orderDetailVModel[0].embarque
                    };

                    ApplicationDbContext.Order.Add(orderModel);
                    ApplicationDbContext.SaveChanges();
                    order.OrderID = orderModel.OrderID;
                    order.Date = orderModel.Date;
                    order.OnShipment = orderModel.OnShipment;

                    //TODO: Crear los detalles del EMBARQUE para la tabla OrderDetails
                    foreach (var item in orderDetailVModel)
                    {
                        for (int i = 0; i < item.total_pallets; i++)
                        {
                            var lastItemOrderDetail = ApplicationDbContext.OrderDetail.LastOrDefault();
                            int idOrderDetail = lastItemOrderDetail == null ? 1 : lastItemOrderDetail.OrderDetailID + 1;
                            orderDetail = new OrderDetailEModel()
                            {
                                OrderDetailID = idOrderDetail,
                                OrderID = orderModel.OrderID,
                                embarque = item.embarque,
                                partida = item.partida,
                                total_pallets = pallet,
                                continentalpartnumber = item.continentalpartnumber,
                                customerpartnumber = item.customerpartnumber,
                                cantidad = item.cantidad,
                                delivery = item.delivery,  
                                traza = item.traza,
                                shipment = item.shipment,
                                notas = item.shipment,
                                Leido = false
                            };
                            pallet++;
                            ApplicationDbContext.OrderDetail.Add(orderDetail);
                            ApplicationDbContext.SaveChanges();
                            var uno = JsonConvert.DeserializeObject<OrderDetailVModel>(JsonConvert.SerializeObject(orderDetail).ToString());
                            orderDetailList.Add(uno);
                        }
                    }
                    ApplicationDbContext.SaveChanges();
                    transaction.Commit();
                }

                order.ListOrderDetail = orderDetailList;
                return order;
            }
            catch (Exception)
            {
                return null;

                //throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
            }
        }


        /// <summary>
        /// Metodo para obtener la información del embarque cuando esta creado pero no iniciado
        /// </summary>
        /// <returns></returns>
        //public async Task<OrderVModel> GetShipmentV2(string shipment)
        //{

        //    return null;
        //}
        #endregion


        public ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }


        #region CreateShipmentV1

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderVModel"></param>
        /// <param name="orderDetailVModel"></param>
        /// <returns></returns>
        //public async Task<OrderVModel> CreateShipment(OrderVModel order, List<OrderDetailVModel> orderDetailVModel)
        //{
        //    try
        //    {
        //        OrderEModel orderModel = new OrderEModel();
        //        OrderDetailEModel orderDetail = new OrderDetailEModel();
        //        List<OrderDetailVModel> orderDetailList = new List<OrderDetailVModel>();


        //        Exist(x => x.ShipmentNumber == order.ShipmentNumber);
        //        orderModel = GetItemByExpression(x => x.ShipmentNumber == order.ShipmentNumber);

        //        var lastItemOrder = Context.Order.LastOrDefault();

        //        using (var transaction = await Context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead))
        //        {
        //            int idOrder = lastItemOrder == null ? 1 : lastItemOrder.OrderID + 1;
        //            if (null == orderModel)
        //            {
        //                orderModel = new OrderEModel()
        //                {
        //                    OrderID = idOrder,
        //                    Date = DateTime.Now,
        //                    OnShipment = false,
        //                    ReaderID = null,
        //                    ShipmentNumber = order.ShipmentNumber
        //                };

        //                Context.Order.Add(orderModel);
        //                Context.SaveChanges();
        //                order.OrderID = orderModel.OrderID;
        //                order.Date = orderModel.Date;
        //                order.OnShipment = orderModel.OnShipment;
        //            }


        //            int pallet = 1;
        //            TODO: Crear los detalles del EMBARQUE
        //            foreach (var item in orderDetailVModel)
        //            {
        //                for (int i = 0; i < item.total_pallets; i++)
        //                {
        //                    var lastItemOrderDetail = Context.OrderDetail.LastOrDefault();
        //                    int idOrderDetail = lastItemOrderDetail == null ? 1 : lastItemOrderDetail.OrderDetailID + 1;
        //                    orderDetail = new OrderDetailEModel()
        //                    {
        //                        OrderDetailID = idOrderDetail,
        //                        OrderID = orderModel.OrderID,
        //                        embarque = item.embarque,
        //                        partida = item.partida,
        //                        total_pallets = pallet,
        //                        continentalpartnumber = item.continentalpartnumber,
        //                        customerpartnumber = item.customerpartnumber,
        //                        cantidad = item.cantidad,
        //                        delivery = item.delivery,
        //                        traza = item.traza,
        //                        shipment = item.shipment,
        //                        notas = item.shipment,
        //                        Leido = false
        //                    };
        //                    pallet++;
        //                    Context.OrderDetail.Add(orderDetail);
        //                    Context.SaveChanges();
        //                    var uno = JsonConvert.DeserializeObject<OrderDetailVModel>(JsonConvert.SerializeObject(orderDetail).ToString());
        //                    orderDetailList.Add(uno);
        //                }
        //            }
        //            Context.SaveChanges();
        //            transaction.Commit();
        //        }
        //        order.ListOrderDetail = orderDetailList;
        //        return order;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
        //    }
        //}

        //public bool Exist(Expression<Func<OrderEModel, bool>> expression) => ReadsItems(expression).Count() > 0;


        //public IQueryable<OrderVModel> GetQueryOrder()
        //{
        //    return ReadsItems().Select(x => new OrderVModel
        //    {
        //        OrderID = x.OrderID,
        //        ReaderID = x.ReaderID,
        //        Number = x.Number,
        //        ShipmentNumber = x.ShipmentNumber,
        //        OnShipment = x.OnShipment,
        //        Date = x.Date,
        //        Finished = x.Finished
        //    });
        //}

        //public IQueryable<OrderVModel> GetQueryOrder(int id)
        //{
        //    return ReadsItems(x => x.OrderID == id).Select(x => new OrderVModel
        //    {
        //        OrderID = x.OrderID,
        //        ReaderID = x.ReaderID,
        //        Number = x.Number,
        //        ShipmentNumber = x.ShipmentNumber,
        //        OnShipment = x.OnShipment,
        //        Date = x.Date,
        //        Finished = x.Finished
        //    });
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public OrderVModel GetQueryOrderComplete(string shipment)
        //{
        //    try
        //    {
        //        List<OrderDetailVModel> query = Context.OrderDetail
        //               .Where(x => x.embarque == shipment).Select(x => new OrderDetailVModel
        //               {
        //                   OrderID = x.OrderID,
        //                   partida = x.partida,
        //                   total_pallets = x.total_pallets,
        //                   continentalpartnumber = x.continentalpartnumber,
        //                   customerpartnumber = x.customerpartnumber,
        //                   traza = x.traza,
        //                   notas = x.notas,
        //                   Leido = x.Leido
        //               }).ToList();

        //        OrderVModel order = ReadsItems(x => x.ShipmentNumber == shipment).Select(x => new OrderVModel
        //        {
        //            OrderID = x.OrderID,
        //            ReaderID = x.ReaderID,
        //            Number = x.Number,
        //            ShipmentNumber = x.ShipmentNumber,
        //            OnShipment = x.OnShipment,
        //            Date = x.Date,
        //            Finished = x.Finished
        //        }).FirstOrDefault();

        //        order.ListOrderDetail = query;
        //        return order;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
        //    }
        //}
        //public OrderVModel GetOrderOnshipment(string embarque)
        //{
        //    try
        //    {
        //        var uno = Context.Reader.ToList();
        //        OrderVModel order = (from orders in ReadsItems(x => x.ShipmentNumber == embarque && x.OnShipment == true)
        //                             join reader in Context.Reader on orders.ReaderID equals reader.ReaderID
        //                             select new
        //                             {
        //                                 orders,
        //                                 reader
        //                             }).Select(x => new OrderVModel
        //                             {
        //                                 OrderID = x.orders.OrderID,
        //                                 ReaderID = x.orders.ReaderID,
        //                                 Number = x.orders.Number,
        //                                 ShipmentNumber = x.orders.ShipmentNumber,
        //                                 OnShipment = x.orders.OnShipment,
        //                                 Date = x.orders.Date,
        //                                 Finished = x.orders.Finished,
        //                             }).FirstOrDefault();
        //        #region Borrar
        //        OrderVModel order = (from orders in ReadsItems(x => x.ShipmentNumber == embarque && x.OnShipment == true)
        //                   join reader in Context.Reader on orders.ReaderID equals reader.ReaderID
        //                   select new
        //                   {
        //                       orders,
        //                       reader
        //                   }).Select(x => new OrderVModel
        //                   {
        //                       OrderID = x.orders.OrderID,
        //                       ReaderID = x.orders.ReaderID,
        //                       Number = x.orders.Number,
        //                       ShipmentNumber = x.orders.ShipmentNumber,
        //                       OnShipment = x.orders.OnShipment,
        //                       Date = x.orders.Date,
        //                       Finished = x.orders.Finished,
        //                       Portal = x.reader.Name
        //                   }).FirstOrDefault();

        //        OrderVModel order = ReadsItems(x => x.ShipmentNumber == embarque && x.OnShipment == true).Select(x => new OrderVModel
        //        {
        //            OrderID = x.OrderID,
        //            ReaderID = x.ReaderID,
        //            Number = x.Number,
        //            ShipmentNumber = x.ShipmentNumber,
        //            OnShipment = x.OnShipment,
        //            Date = x.Date,
        //            Finished = x.Finished
        //        }).FirstOrDefault(); 
        //        #endregion
        //        return order;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
        //    }
        //}

        //public OrderLigthVModel GetOrderById(int idOrder)
        //{
        //    try
        //    {

        //        OrderLigthVModel order = (from orders in ReadsItems(x => x.OrderID == idOrder && x.OnShipment == true)
        //                                  join reader in Context.Reader on orders.ReaderID equals reader.ReaderID
        //                                  select new
        //                                  {
        //                                      orders,
        //                                      reader
        //                                  }).Select(x => new OrderLigthVModel
        //                                  {
        //                                      OrderID = x.orders.OrderID,
        //                                      ReaderID = x.orders.ReaderID,
        //                                      Portal = x.reader.Name
        //                                  }).FirstOrDefault();

        //        return order;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
        //    }
        //}

        //public OrderEModel GetOrderEModel(string embarque)
        //{
        //    try
        //    {
        //        OrderEModel order = ReadsItems(x => x.ShipmentNumber == embarque && x.OnShipment == true).FirstOrDefault();
        //        return order;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
        //    }
        //}

        //public async Task<OrderEModel> PutOrderEModel(OrderLigthVModel model)
        //{
        //    try
        //    {
        //        OrderEModel order = await ReadsItems(x => x.OrderID == model.OrderID).FirstOrDefaultAsync();
        //        order.ReaderID = model.ReaderID;
        //        order.Date = DateTime.Now;
        //        Context.Order.Update(order);
        //        return order;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
        //    }
        //}

        //public OrderVModel GetOrderByOrderId(int id)
        //{
        //    try
        //    {

        //        OrderVModel order = (from orders in ReadsItems(x => x.OrderID == id)
        //                             join reader in Context.Reader on orders.ReaderID equals reader.ReaderID
        //                             select new
        //                             {
        //                                 orders,
        //                                 reader
        //                             }).Select(x => new OrderVModel
        //                             {
        //                                 OrderID = x.orders.OrderID,
        //                                 ReaderID = x.orders.ReaderID,
        //                                 Number = x.orders.Number,
        //                                 ShipmentNumber = x.orders.ShipmentNumber,
        //                                 OnShipment = x.orders.OnShipment,
        //                                 Date = x.orders.Date,
        //                                 Finished = x.orders.Finished,
        //                                 Portal = x.reader.Name
        //                             }).FirstOrDefault();

        //        return order;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new DataValidationException("Error", string.Format("No fué posible crear el embarque: {0}", ex.Message));
        //    }
        //} 
        #endregion

    }
}
