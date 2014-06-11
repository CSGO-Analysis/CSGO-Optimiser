using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Player : IPlayer
    {
        public string Name { get; set; }
        public string CfgPath { get; set; }
        public string VideoPath { get; set; }

        public Player(string name, string cfgPath, string videoPath)
        {
            Name = name;
            CfgPath = cfgPath;
            VideoPath = videoPath;
        }
    }
}
