using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITNESS.DatabaseModel
{
    class MilightCombo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int FirstGroupId { get; set; }
        public int SecondGroupId { get; set; }
        public int Brightness { get; set; } = 27; // 39 is max
        public int Color { get; set; } = 0xFF;
        public bool LastState { get; set; } = false;
    }
}
