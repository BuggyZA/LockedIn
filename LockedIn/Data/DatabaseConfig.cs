using System;

namespace LockedIn.Data
{
    internal class DatabaseConfig
    {
        public static String GetConStr()
        {
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LockedInDB.mdf;Integrated Security=True";
        }
    }
}
