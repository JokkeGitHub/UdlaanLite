using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaansSystem
{
    class ObjectPC
    {
        private string _qrID;
        private string _serialNumber;
        private string _pcModel;
        private bool _inStock;

        public string QRID
        {
            get
            {
                return this._qrID;
            }
            set
            {
                this._qrID = value;
            }
        }

        public string SerialNumber
        {
            get
            {
                return this._serialNumber;
            }
            set
            {
                this._serialNumber = value;
            }
        }

        public string PCModel
        {
            get
            {
                return this._pcModel;
            }
            set
            {
                this._pcModel = value;
            }
        }

        public bool InStock
        {
            get
            {
                return this._inStock;
            }
            set
            {
                this._inStock = value;
            }
        }

        public ObjectPC(string _qrID, string _serialNumber, string _pcModel, bool _inStock)
        {
            QRID = _qrID;
            SerialNumber = _serialNumber;
            PCModel = _pcModel;
            InStock = _inStock;
        }
    }
}
