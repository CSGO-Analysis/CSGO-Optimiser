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
        string CfgPath { get; set; }
        string VideoPath { get; set; }
    }
}
