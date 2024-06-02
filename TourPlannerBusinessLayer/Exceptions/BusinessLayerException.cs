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

    public class DirectionsServiceException : BusinessLayerException
    {
        public DirectionsServiceException() { }

        public DirectionsServiceException(string message) : base(message) { }

        public DirectionsServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class RouteDataManagerException : BusinessLayerException
    {
        public RouteDataManagerException() { }

        public RouteDataManagerException(string message) : base(message) { }

        public RouteDataManagerException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ReportManagerException : BusinessLayerException
    {
        public ReportManagerException() { }

        public ReportManagerException(string message) : base(message) { }

        public ReportManagerException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class FileTransferManagerException : BusinessLayerException
    {
        public FileTransferManagerException() { }

        public FileTransferManagerException(string message) : base(message) { }

        public FileTransferManagerException(string message, Exception innerException) : base(message, innerException) { }
    }
}

