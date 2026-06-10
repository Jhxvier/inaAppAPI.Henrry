using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Exceptions
{
    public class InvalidStockException : Exception
    {
        public InvalidStockException()
        {
        }
        public InvalidStockException(string? message) : base(message)
        {
        }
    }
}
