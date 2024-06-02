using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerBusinessLayer.Exceptions
{
    public class BusinessLayerException : Exception
    {
        public BusinessLayerException() { }

        public BusinessLayerException(string message) : base(message) { }

        public BusinessLayerException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ValidationException : BusinessLayerException
    {
        public ValidationException() { }

        public ValidationException(string message) : base(message) { }

        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class TourServiceException : BusinessLayerException
    {
        public TourServiceException() { }

        public TourServiceException(string message) : base(message) { }

        public TourServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class TourLogServiceException : BusinessLayerException
    {
        public TourLogServiceException() { }

        public TourLogServiceException(string message) : base(message) { }

        public TourLogServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class GeocodeServiceException : BusinessLayerException
    {
        public GeocodeServiceException() { }

        public GeocodeServiceException(string message) : base(message) { }

        public GeocodeServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}

