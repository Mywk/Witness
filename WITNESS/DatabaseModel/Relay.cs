using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITNESS.DatabaseModel
{
    class Relay
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Gpio { get; set; }
        public bool TimerActive { get; set; }
        public bool LastState { get; set; } = false;
    }
}
