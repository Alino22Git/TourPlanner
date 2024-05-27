using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerConfig
{
    public interface IConfigurationService
    {
        string GetConnectionString();
        T GetValue<T>(string key);
    }

}
