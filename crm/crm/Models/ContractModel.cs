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

        public ContractModel(LegalPersonModel legalPersonModel, NaturalPersonModel naturalPersonModel, string summContract, string status, string dateOfSingning)
        {
            _legalPersonModel = legalPersonModel;
            _naturalPersonModel = naturalPersonModel;
            _summContract = summContract;
            _status = status;
            _dateOfSingning = dateOfSingning;
        }

        #endregion

        #region Fields

        private LegalPersonModel _legalPersonModel;
        private NaturalPersonModel _naturalPersonModel;
        private string _summContract;
        private string _status;
        private string _dateOfSingning;

        #endregion

        #region Properties

        public LegalPersonModel LegalPersonModel { get => _legalPersonModel; set => _legalPersonModel = value; }
        public NaturalPersonModel NaturalPersonModel { get => _naturalPersonModel; set => _naturalPersonModel = value; }
        public string SummContract { get => _summContract; set => _summContract = value; }
        public string Status { get => _status; set => _status = value; }
        public string DateOfSingning { get => _dateOfSingning; set => _dateOfSingning = value; }

        #endregion
    }
}
