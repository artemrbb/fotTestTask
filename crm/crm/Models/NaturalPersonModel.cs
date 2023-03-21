using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models
{
    class NaturalPersonModel
    {
        #region Constructor

        public NaturalPersonModel(string name, string surname, string patronymic, string gender, string age, string placeOfWork, string contry, string city, string address, string email, string phoneNumber, string dateOfBirth)
        {
            _name = name;
            _surname = surname;
            _patronymic = patronymic;
            _gender = gender;
            _age = age;
            _placeOfWork = placeOfWork;
            _contry = contry;
            _city = city;
            _address = address;
            _email = email;
            _phoneNumber = phoneNumber;
            _dateOfBirth = dateOfBirth;
        }

        #endregion

        #region Fields

        private string _name;
        private string _surname;
        private string _patronymic;
        private string _gender;
        private string _age;
        private string _placeOfWork;
        private string _contry;
        private string _city;
        private string _address;
        private string _email;
        private string _phoneNumber;
        private string _dateOfBirth;

        #endregion

        #region Properties

        public string Name { get => _name; set => _name = value; }
        public string Surname { get => _surname; set => _surname = value; }
        public string Patronymic { get => _patronymic; set => _patronymic = value; }
        public string Gender { get => _gender; set => _gender = value; }
        public string Age { get => _age; set => _age = value; }
        public string PlaceOfWork { get => _placeOfWork; set => _placeOfWork = value; }
        public string Contry { get => _contry; set => _contry = value; }
        public string City { get => _city; set => _city = value; }
        public string Address { get => _address; set => _address = value; }
        public string Email { get => _email; set => _email = value; }
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
        public string DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }

        #endregion
    }
}
