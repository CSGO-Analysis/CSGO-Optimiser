using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IBackup : IProfile
    {
        Guid Id { get; set; }
        DateTime Timestamp { get; set; }
        List<string> Localconfigs { get; set; }
    }
}
