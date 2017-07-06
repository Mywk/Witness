using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITNESS.DatabaseModel
{
    class Timer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TargetId { get; set; }
        public int Type { get; set; }
        public double From { get; set; }
        public double To { get; set; }
    }
}
