using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models
{
    class LegalPersonModel
    {
        #region Constructor

        public LegalPersonModel(string nameCompany, string inn, string ogrn, string contry, string city, string address, string email, string phoneNumber)
        {
            _nameCompany = nameCompany;
            _inn = inn;
            _ogrn = ogrn;
            _contry = contry;
            _city = city;
            _address = address;
            _email = email;
            _phoneNumber = phoneNumber;
        }

        #endregion

        #region Fields


        private string _nameCompany;
        private string _inn;
        private string _ogrn;
        private string _contry;
        private string _city;
        private string _address;
        private string _email;
        private string _phoneNumber;

        #endregion

        #region Properties

        public string NameCompany { get => _nameCompany; set => _nameCompany = value; }
        public string Inn { get => _inn; set => _inn = value; }
        public string Ogrn { get => _ogrn; set => _ogrn = value; }
        public string Contry { get => _contry; set => _contry = value; }
        public string City { get => _city; set => _city = value; }
        public string Address { get => _address; set => _address = value; }
        public string Email { get => _email; set => _email = value; }
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }

        #endregion
    }
}
