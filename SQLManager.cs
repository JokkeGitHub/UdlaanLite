using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaansSystem
{
    class SQLManager
    { 
        // Lav noget der henter værdier fra XAML pages, eller gør det som i landlyst
        public static string qrID;
        public static string serialNumber;
        public static string pcModel;
        public static bool inStock;

        public static void RegisterPC()
        {
            ObjectPC addPC = new ObjectPC(qrID, serialNumber, pcModel, inStock);

            RegisterSQLConnections.CreatePC(addPC.QRID, addPC.SerialNumber, addPC.PCModel, addPC.InStock);
        }


        public static string uniLogin;
        public static string name;
        public static string phoneNumber;
        public static bool isStudent;

        public static DateTime startDate;
        public static DateTime endDate;

        public static void ExportToLoaner()
        {
            ObjectLoaner addLoaner = new ObjectLoaner(uniLogin, name, phoneNumber, isStudent);
            ExportSQLConnections.CreateLoaner(addLoaner.UNILogin, addLoaner.Name, addLoaner.PhoneNumber, addLoaner.IsStudent);

            ObjectLoan addLoan = new ObjectLoan(startDate, endDate);
            ExportSQLConnections.CreateLoan(addLoan.StartDate, addLoan.EndDate);
        }
    }
}
