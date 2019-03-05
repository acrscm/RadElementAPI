using System;

namespace RadElement.Core.Services.Exception
{
    /// <summary>
    /// Exception thrown  when a resource is  not found
    /// </summary>
    public class ResourceNotFoundException : ApplicationException
    {
        public ResourceNotFoundException(string message) : base(message)
        {
        }
    }
}
