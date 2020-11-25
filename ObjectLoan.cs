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

        private DateTime _startDate;
        private DateTime _endDate;

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

        public DateTime EndDate
        {
            get
            {
                return this._endDate;
            }
            set
            {
                this._endDate = value;
            }
        }

        public ObjectLoan(string _uniLogin, string _qrId, DateTime _startDate, DateTime _endDate)
        {
            // Måske find ud af at få foreign keys med
            UNILogin = _uniLogin;
            QRID = _qrId;
            StartDate = _startDate;
            EndDate = _endDate;
        }
    }
}
