using System;
using System.Collections.Generic;
using System.Text;

namespace DESUBot.Models
{
    class UtilityRoles
    {
        public static string PicPermDisable
        {
            get { return "cant post pics"; }
        }
        public static string Muted
        {
            get { return "Muted"; }
        }
        public static string Moderator
        {
            get { return "mod"; }
        }
        public static string Admin
        {
            get { return "adnim"; }
        }
        public static string Member
        {
            get { return "member";  }
        }
        public static string Delete
        {
            get { return "delete";  }
        }
    }
}
