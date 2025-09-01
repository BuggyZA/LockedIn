using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockedIn.Data
{
    public static class SessionData
    {
        public static int LoggedInPrisonerID { get; set; }
        public static int LoggedInEmployerID { get; set; }
        public static int SelectedUser { get; set; }

        public static class SessionEmployerID
        {
            public static int EmployerID { get; set; }
            public static string FirstName { get; set; }
        }


    }
}
