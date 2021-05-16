using System;
namespace KR_Lib.Exceptions
{
    public class InconsistentException : Exception
    {
        public InconsistentException()
        {
        }

        public InconsistentException(string message)
            : base(message)
        {
        }

        public InconsistentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
