using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Exceptions
{
    public class InvalidClientNameException : Exception
    {
        public InvalidClientNameException() { }
        public InvalidClientNameException(string? message) : base(message) { }
    }
}
