using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    static class clsDataAccessSettings
    {
        public static string connectionString = "Server=.;Database=ContactsDB;User Id=sa;Password=123456";

        public static void PrintExecptionErrorMessage(Exception ex)
        {
            Console.WriteLine("Error : " + ex.Message);
            Console.ReadKey();
        }
    }
}
