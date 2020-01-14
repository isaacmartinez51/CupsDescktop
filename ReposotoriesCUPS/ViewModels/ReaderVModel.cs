using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposotoriesCUPS.ViewModels
{
    public class ReaderVModel
    {
        public int ReaderID { get; set; }
        public int ReaderKindID { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string Serial { get; set; }
    }
}
