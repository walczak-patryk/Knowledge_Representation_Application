using System;
namespace KR_Lib.Exceptions
{
    public class ScenarioNoExistsException : Exception
    {
        public ScenarioNoExistsException()
        {
        }

        public ScenarioNoExistsException(string message)
            : base(message)
        {
        }

        public ScenarioNoExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
