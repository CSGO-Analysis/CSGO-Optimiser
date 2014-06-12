using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Backup : Profile, IBackup
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }

        public Backup(Guid id, DateTime timestamp, string config, string crosshair, string autoexec, string videoSettings, string launchOptions)
        {
            Id = id;
            Timestamp = timestamp;
            // fix base:
            Config = config;
            Crosshair = crosshair;
            Autoexec = autoexec;
            VideoSettings = videoSettings;
            LaunchOptions = launchOptions;
        }

        public string[] TxtFile()
        {
            return new string[] { "Id = " + Id, "Timestamp = " + Timestamp, "Config = " + Config, "Crosshair = " + Crosshair,
                "Autoexec = " + Autoexec, "VideoSettings = " + VideoSettings, "LaunchOptions = " + LaunchOptions };
        }
    }
}
