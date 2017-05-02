using System;

namespace Template10.Services.ViewService
{
	public class ViewLifeTimeException : Exception
    {
        public ViewLifeTimeException()
        {
        }

        public ViewLifeTimeException(string message) : base(message)
        {
        }

        public ViewLifeTimeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}