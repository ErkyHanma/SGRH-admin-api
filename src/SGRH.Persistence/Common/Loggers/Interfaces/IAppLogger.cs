using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGRH.Persistence.Common.Loggers.Interfaces
{
    public interface IAppLogger<T>
    {
       void Info(string message, params object[] args);
       void ErrorNoEx(string message, params object[] args);
       void ErrorEx(Exception ex, string message, params object[] args);
    }
}
