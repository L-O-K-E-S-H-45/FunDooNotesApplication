using System;
using System.Collections.Generic;
using System.Text;

namespace ExcepionHandling.CustomExceptions
{
    public class IllegalRequestException : Exception
    {
        public IllegalRequestException(string message) : base(message) { }

    }
}
