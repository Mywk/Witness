using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITNESS.DatabaseModel
{
    class Timers
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public byte[] Time { get; set; }
    }
}
