using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITNESS
{
    public sealed class Database
    {
        internal static Database Active;

        private SQLiteConnection conn = null;

        public Database()
        {
            string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            FileInfo file = new FileInfo(path);

            // For debugging
            bool alwaysDelete = true;
            bool testEntries = true;

            if (alwaysDelete)
            {
                if (file.Exists)
                    file.Delete();
            }

            if (!file.Exists || alwaysDelete)
            {
                conn = new SQLiteConnection(path);

                try
                {
                    conn.CreateTable<DatabaseModel.Timer>(CreateFlags.None);
                    conn.CreateTable<DatabaseModel.Relay>(CreateFlags.None);
                    conn.CreateTable<DatabaseModel.MilightBridge>(CreateFlags.None);
                    conn.CreateTable<DatabaseModel.MilightGroup>(CreateFlags.None);
                    conn.CreateTable<DatabaseModel.MilightCombo>(CreateFlags.None);

                    if (testEntries)
                    {
                        // Looks bad but will suffice for now
                        conn.Insert(new DatabaseModel.Relay()
                        {
                            Gpio = 27,
                            Name = "TV"
                        });
                        conn.Insert(new DatabaseModel.Relay()
                        {
                            Gpio = 17,
                            Name = "Monitor 1 & 2"
                        });
                        conn.Insert(new DatabaseModel.Relay()
                        {
                            Gpio = 22,
                            Name = "Plant Lights",
                            TimerActive = true
                        });
                        conn.Insert(new DatabaseModel.Timer()
                        {
                            TargetId = 3,
                            From = 28800,
                            To = 75600,
                            Type = (int)Enums.TimerType.Relay
                        });
                        conn.Insert(new DatabaseModel.MilightBridge()
                        {
                            Id = 1,
                            Name = "Bridge 1",
                            Ip = "192.168.1.111"
                        });
                        conn.Insert(new DatabaseModel.MilightGroup()
                        {
                            Id = 1,
                            Name = "Group 1",
                            GroupType = 0,
                            BridgeId = 1
                        });
                        conn.Insert(new DatabaseModel.MilightGroup()
                        {
                            Id = 2,
                            Name = "Group 2",
                            GroupType = 1,
                            BridgeId = 1
                        });
                        conn.Insert(new DatabaseModel.MilightGroup()
                        {
                            Name = "Bedroom",
                            GroupType = 2,
                            BridgeId = 1
                        });
                        conn.Insert(new DatabaseModel.MilightCombo()
                        {
                            Name = "Living room",
                            FirstGroupId = 1,
                            SecondGroupId = 2
                        });
                    }
                }
                catch (Exception e)
                {

                    throw;
                }

            }
            else
                conn = new SQLiteConnection(path);
        }

        internal SQLiteConnection GetConnection()
        {
            return conn;
        }
    }
}
