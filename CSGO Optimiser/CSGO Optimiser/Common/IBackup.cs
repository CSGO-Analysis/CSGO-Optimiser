using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IBackup
    {
        Guid Id { get; set; }
        DateTime Timestamp { get; set; }
        //string CapsLockReg { get; set; }
        //string DefaultMouseReg { get; set; }
        //string MouseKeyReg { get; set; }
        //string VisualThemes { get; set; }
    }
}
