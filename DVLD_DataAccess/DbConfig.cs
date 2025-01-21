using System;
using System.Configuration;

namespace DVLD_DataAccess
{
    static class DbConfig
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
        }
    }
}
