using System;

namespace Reddit.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) 
        {
            Error = new ServiceError { Message = message };
        }

        // This provides a structured error
        public ServiceError Error { get; }

        public class ServiceError
        {
            public string Message { get; set; }
        }
    }
}
