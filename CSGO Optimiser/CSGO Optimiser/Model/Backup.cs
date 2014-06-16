using Common;
using System;
using System.Collections.Generic;

namespace Model
{
    public class Backup : Profile, IBackup
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public List<string> Localconfigs { get; set; }

        public Backup(Guid id, DateTime timestamp)
        {
            Id = id;
            Timestamp = timestamp;
            Localconfigs = new List<string>();
        }

        public string[] ToStringArray()
        {
            return new string[] { "Id = " + Id, "Timestamp = " + Timestamp };
        }
    }
}
