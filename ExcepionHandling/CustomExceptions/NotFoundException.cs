using System;
using System.Collections.Generic;
using System.Text;

namespace ExcepionHandling.CustomExceptions
{
    public class NotFoundException : Exception
    {

        public NotFoundException(string message) : base(message) { }

    }
}
