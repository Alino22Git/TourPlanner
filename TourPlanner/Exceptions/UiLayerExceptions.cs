using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Exceptions
{
    public class UiLayerException : Exception
    {
        public UiLayerException() { }

        public UiLayerException(string message) : base(message) { }

        public UiLayerException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InitializationException : UiLayerException
    {
        public InitializationException() { }

        public InitializationException(string message) : base(message) { }

        public InitializationException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UpdateException : UiLayerException
    {
        public UpdateException() { }

        public UpdateException(string message) : base(message) { }

        public UpdateException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class SaveException : UiLayerException
    {
        public SaveException() { }

        public SaveException(string message) : base(message) { }

      public SaveException(string message, Exception innerException) : base(message, innerException) { }
    }
}
