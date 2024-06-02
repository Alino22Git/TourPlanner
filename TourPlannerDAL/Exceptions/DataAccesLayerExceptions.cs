using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerDAL.Exceptions
{
    public class DataAccessLayerException : Exception
    {
        public DataAccessLayerException() { }

        public DataAccessLayerException(string message) : base(message) { }

        public DataAccessLayerException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class TourRepositoryException : DataAccessLayerException
    {
        public TourRepositoryException() { }

        public TourRepositoryException(string message) : base(message) { }

        public TourRepositoryException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class TourLogRepositoryException : DataAccessLayerException
    {
        public TourLogRepositoryException() { }

        public TourLogRepositoryException(string message) : base(message) { }

        public TourLogRepositoryException(string message, Exception innerException) : base(message, innerException) { }
    }
}