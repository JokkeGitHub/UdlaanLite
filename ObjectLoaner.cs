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
        private string _comment;
        private string _phone;
        private int _isStudent;

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
        public string Phone
        {
            get
            {
                return this._phone;
            }
            set
            {
                this._phone = value;
            }
        }

        public int IsStudent
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

        public ObjectLoaner(string _uniLogin, string _name, string _comment, string _phone, int _isStudent)
        {
            UNILogin = _uniLogin;
            Name = _name;
            Comment = _comment;
            Phone = _phone;
            IsStudent = _isStudent;
        }
    }
}
