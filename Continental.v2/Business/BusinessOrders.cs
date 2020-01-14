using Repositories.Data.Entities;
using Repositories.ViewModels;
using ReposotoriesCUPS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continental.v2.Business
{
    public static class BusinessOrders
    {
        public static bool Terminado(string embarque)
        {
            bool terminado = false;
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
                terminado = unitOfWork.OrderR.Exist(x => x.ShipmentNumber.Equals(embarque) && x.OnShipment == true && x.Finished == true);
            }
            return terminado;
        }

        public static bool ExisteNoAsignada(string embarque) {
            bool existe = false;
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
                var todo = unitOfWork.OrderR.ReadsItems();
                existe = unitOfWork.OrderR.Exist(x => x.ShipmentNumber.Equals(embarque) && x.OnShipment == false);
            }
            return existe;
        }
        /// <summary>
        /// Existe y esta asignada
        /// </summary>
        /// <param name="embarque"></param>
        /// <returns></returns>
        public static bool ExisteAsignada(string embarque)
        {
            bool existe = false;
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
                existe = unitOfWork.OrderR.Exist(x => x.ShipmentNumber.Equals(embarque) && x.OnShipment == true);
         
            }
            return existe;
        }

        public static OrderVModel GetOrder(string embarque)
        {
            OrderVModel orden = new OrderVModel();
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
                orden = unitOfWork.OrderR.GetOrderOnshipment(embarque);
            }
            return orden;
        }

        public static OrderVModel GetOrdenCompleta(string embarque)
        {
            OrderVModel orden = new OrderVModel();
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
                orden = unitOfWork.OrderR.GetQueryOrderComplete(embarque);
            }
            return orden;
        }

        public static async Task<OrderVModel> CreateNuevaOrden(ShipmentVModel embarque)
        {
            OrderVModel orden = new OrderVModel();
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
              
                orden = await unitOfWork.OrderR.CreateShipmentV2(embarque.detalle);
                 
            }
            return orden;
        }

        public static int IniciarEmbarque(string embarque, int anden) 
        {
            OrderEModel orden = new OrderEModel();
            int uno = 0;
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {

                var ordenOld = unitOfWork.OrderR.ReadsItems(x=> x.ShipmentNumber == embarque).FirstOrDefault();
                ordenOld.ReaderID = anden;
                ordenOld.OnShipment = true;

                uno = unitOfWork.Complete();

                
                // orden = await unitOfWork.OrderR.UpdateItemAsync(ordenOld);

            }
            return uno;
        }
        public static int TerminarEmbarque(string embarque)
        {
            OrderEModel orden = new OrderEModel();
            int uno = 0;
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {

                var ordenOld = unitOfWork.OrderR.ReadsItems(x => x.ShipmentNumber == embarque).FirstOrDefault();
                ordenOld.Finished = true;
                ordenOld.OnShipment = true;

                uno = unitOfWork.Complete();


                // orden = await unitOfWork.OrderR.UpdateItemAsync(ordenOld);

            }
            return uno;
        }
        public static List<OrderDetailEModel> EmbarqueVivo(string embarque) {
            List<OrderDetailEModel> orden = new List<OrderDetailEModel>();
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
                orden = unitOfWork.OrderDetailR.ReadsItems(x => x.embarque == embarque && x.Leido == false).ToList();
            }
            return orden;
        }
        public static bool EmbarqueVivo2(string embarque)
        {
            List<OrderDetailEModel> orden = new List<OrderDetailEModel>();
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
                orden = unitOfWork.OrderDetailR.ReadsItems(x => x.embarque == embarque && x.Leido == false).ToList();
            }
            bool vivo = orden.Count > 0 ? true : false;
            return vivo;
        }
        public static int MarcarLeido(string embarque, string numparte)
        {
            
            int uno = 0;
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {

                var ordenOld = unitOfWork.OrderDetailR.ReadsItems(x => x.embarque == embarque && x.continentalpartnumber == numparte && x.Leido == false).FirstOrDefault();
                if (ordenOld != null)
                {
                    ordenOld.Leido = true;

                    uno = unitOfWork.Complete();
                }
                // orden = await unitOfWork.OrderR.UpdateItemAsync(ordenOld);

            }
            return uno;
        }

    }
}
