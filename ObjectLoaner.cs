using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaansSystem
{
    class ObjectLoaner
    {
        private string _uniLogin;
        private string _name;
        private string _phoneNumber;
        private bool _isStudent;

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
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }
        public string PhoneNumber
        {
            get
            {
                return this._phoneNumber;
            }
            set
            {
                this._phoneNumber = value;
            }
        }

        public bool IsStudent
        {
            get
            {
                return this._isStudent;
            }
            set
            {
                this._isStudent = value;
            }
        }

        public ObjectLoaner(string _uniLogin, string _name, string _phonenUmber, bool _isStudent)
        {
            UNILogin = _uniLogin;
            Name = _name;
            PhoneNumber = _phoneNumber;
            IsStudent = _isStudent;
        }
    }
}
