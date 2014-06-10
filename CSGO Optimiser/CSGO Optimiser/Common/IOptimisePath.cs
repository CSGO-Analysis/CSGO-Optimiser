using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IOptimisePath
    {
        string Steam { get; set; }
        string CsgoExe { get; }
        string Cfg { get; }
    }
}
