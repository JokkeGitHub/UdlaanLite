using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaansSystem
{
    class ObjectLoan
    {
        private string _uniLogin;
        private string _qrId;
        private string _comment;
        private DateTime _startDate;

        public string UNILogin
        {
            get
            {
                return this._uniLogin;
            }
            set
            {
                this._uniLogin = value;
            }
        }
        public string QRID
        {
            get
            {
                return this._qrId;
            }
            set
            {
                this._qrId = value;
            }
        }
        public string Comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this._startDate;
            }
            set
            {
                this._startDate = value;
            }
        }

        public ObjectLoan(string _uniLogin, string _qrId, string _comment, DateTime _startDate)
        {
            // Måske find ud af at få foreign keys med
            UNILogin = _uniLogin;
            QRID = _qrId;
            Comment = _comment;
            StartDate = _startDate;
        }
    }
}
