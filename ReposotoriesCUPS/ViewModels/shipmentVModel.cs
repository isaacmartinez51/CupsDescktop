using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.ViewModels
{
    public class ShipmentVModel
    {
        public int estatus { get; set; }

        public string cancelado { get; set; }

        public List<OrderDetailVModel> detalle { get; set; }
    }
}
