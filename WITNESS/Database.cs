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
                    conn.CreateTable<DatabaseModel.Timers>(CreateFlags.None);
                    conn.CreateTable<DatabaseModel.Relays>(CreateFlags.None);

                    if (testEntries)
                    {
                        // Looks bad but will suffice for now
                        conn.Insert(new DatabaseModel.Relays()
                        {
                            Gpio = 17,
                            Name = "TV",
                            DefaultState = false
                        });
                        conn.Insert(new DatabaseModel.Relays()
                        {
                            Gpio = 27,
                            Name = "Monitor 1 & 2",
                            DefaultState = true
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
