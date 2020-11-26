using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaansSystem
{
    class SQLManager
    { 
        public static void RegisterPC(string qrID, string serialNumber, string pcModel)
        {
            ObjectPC addPC = new ObjectPC(qrID, serialNumber, pcModel);

            RegisterSQLConnections.CreatePC(addPC.QRID, addPC.SerialNumber, addPC.PCModel);
        }

        public static void CreateLoaner(string uniLogin, string name, string phone, int isStudent)
        {
            ObjectLoaner addLoaner = new ObjectLoaner(uniLogin, name, phone, isStudent);
            ExportSQLConnections.CreateLoaner(addLoaner.UNILogin, addLoaner.Name, addLoaner.Phone, addLoaner.IsStudent);
        }

        public static void CreateLoan(string uniLogin, string qrId, DateTime startDate, DateTime endDate)
        {
            ObjectLoan addLoan = new ObjectLoan(uniLogin, qrId, startDate, endDate);
            ExportSQLConnections.CreateLoan(addLoan.UNILogin, addLoan.QRID, addLoan.StartDate, addLoan.EndDate);
        }

        #region CHECKING DATABASE FOR DATA
        public static bool CheckUniLogin(string uniLogin)
        {
            bool uniLoginExists  = ExportSQLConnections.CheckDatabaseForLogin(uniLogin);
            return uniLoginExists;
        }
        #endregion

        #region GETTING LOAN INFO
        public static string GetActiveStudentLoanInfo(string uniLogin)
        {
            string activeLoanInfo = ExportSQLConnections.GetLoanInfo(uniLogin);

            return activeLoanInfo;
        }
        #endregion
    }
}
