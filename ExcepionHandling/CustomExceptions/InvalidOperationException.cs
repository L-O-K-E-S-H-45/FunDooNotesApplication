using System;
using System.Collections.Generic;
using System.Text;

namespace ExcepionHandling.CustomExceptions
{
    public class InvalidOperationException : Exception
    {
        public InvalidOperationException(string message) : base(message) { }

    }
}
