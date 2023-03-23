using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models
{
    class ContractModel
    {
        #region Constructor

        public ContractModel(string legalPersonModel, string naturalPersonModel, string summContract, string status, DateTime dateOfSingning)
        {
            _legalPersonModel = legalPersonModel;
            _naturalPersonModel = naturalPersonModel;
            _summContract = summContract;
            _status = status;
            _dateOfSingning = dateOfSingning;
        }

        #endregion

        #region Fields

        private string _legalPersonModel;
        private string _naturalPersonModel;
        private string _summContract;
        private string _status;
        private DateTime _dateOfSingning;

        #endregion

        #region Properties

        public string LegalPersonModel { get => _legalPersonModel; set => _legalPersonModel = value; }
        public string NaturalPersonModel { get => _naturalPersonModel; set => _naturalPersonModel = value; }
        public string SummContract { get => _summContract; set => _summContract = value; }
        public string Status { get => _status; set => _status = value; }
        public DateTime DateOfSingning { get => _dateOfSingning; set => _dateOfSingning = value; }

        #endregion
    }
}
