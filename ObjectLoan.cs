using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaansSystem
{
    class ObjectLoan
    {
        private DateTime _startDate;
        private DateTime _endDate;

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

        public ObjectLoan(DateTime _startDate, DateTime _endDate)
        {
            // Måske find ud af at få foreign keys med
            StartDate = _startDate;
            EndDate = _endDate;
        }
    }
}
