using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continental.v2.Classes.Exceptions
{
    public class DataValidationException : Exception
    {
        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }

        public DataValidationException(string propertyName, string errorMessage)
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
        }
    }
}
