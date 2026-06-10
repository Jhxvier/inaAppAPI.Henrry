using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Exceptions
{
    public class DuplicateIdentificationException : Exception
    {
        public DuplicateIdentificationException() { }
        public DuplicateIdentificationException(string? message) : base(message) { }
    }
}
