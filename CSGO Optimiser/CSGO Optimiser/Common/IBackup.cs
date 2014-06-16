using System;
using System.Collections.Generic;

namespace Common
{
    public interface IBackup : IProfile
    {
        Guid Id { get; set; }
        DateTime Timestamp { get; set; }
        List<string> Localconfigs { get; set; }
    }
}
