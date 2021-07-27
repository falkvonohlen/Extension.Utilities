using System;
using System.Collections.Generic;
using System.Text;

namespace Extension.Utilities.Exceptions
{
    public class InvalidFileExtension : Exception
    {
        public InvalidFileExtension(string msg) : base(msg) { }
    }
}
