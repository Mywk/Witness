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
                            From = 1234,
                            To = 2456,
                            Type = (int)Enums.TimerType.Relay
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
