using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IPlayer
    {
        string Name { get; set; }
        string Config { get; set; }
        string Crosshair { get; set; }
        string Autoexec { get; set; }
        string VideoSettings { get; set; }
        string LaunchOptions { get; set; }
        string NvidiaProfile { get; set; }
        bool DisabledMouseAcc { get; set; }
        bool DisabledIngameMouseAcc { get; set; }
        bool DisabledCapsLock { get; set; }
        bool DisabledVisualThemes { get; set; }
        string FolderPath { get; set; }
    }
}
