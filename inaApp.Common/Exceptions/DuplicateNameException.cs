using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Exceptions
{
    public class DuplicateNameException : Exception
    {
        public DuplicateNameException() { }
        public DuplicateNameException(string? message) : base(message) { }
    }
}
