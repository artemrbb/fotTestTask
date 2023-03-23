using crm.Models;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateCore.AppManagement;
using UltimateCore.LRI;

namespace crm.DB
{
    sealed class DBMaster : Singleton<DBMaster>
    {
        #region Const

        private const string NATURAL_PERSONS_TABLE = "NaturalPersons";
        private const string LEGAL_PERSONS_TABLE = "LegalPersons";
        private const string CONTRACTS_TABLE = "Contracts";

        #endregion

        #region Constructor 

        private DBMaster()
        {

        }

        #endregion

        #region Fields 

        private NpgsqlConnection _npgsqlConnection;

        #endregion

        #region Methods 

        public Result<bool> Connection(string host, string username, string password, string database) 
        {
            return new Result<bool>(() =>
            {
                var resConnection = new UltimateCore.LRI.Result<bool>(() =>
                {
                    _npgsqlConnection = new NpgsqlConnection($"Host={host};Username={username};Password={password};Database={database}"); // $"Host={host};Username={username};Password={password};Database={database}"
                    _npgsqlConnection.Open();
                    return true;
                });
                if (!resConnection.IsOk)
                    throw new Exception($"При подключении произошла ошибка: {resConnection.ErrorMessage}");

                var resCreateBaseTables = new Result<NpgsqlCommand>(() =>
                {
                    return new NpgsqlCommand($"" +
                        $"create table if not exists {LEGAL_PERSONS_TABLE} (name_company varchar(100), inn varchar(100), ogrn varchar(100), contry varchar(100), city varchar(100), address varchar(100), email varchar(100), phone_number varchar(100));" +
                        $"create table if not exists {NATURAL_PERSONS_TABLE} (name varchar(100),surname varchar(100)," +
                        $"patronymic varchar(100)," +
                        $"gender varchar(100)," +
                        $"age int," +
                        $"place_of_work varchar(100)," +
                        $"contry varchar(100)," +
                        $"city varchar(100)," +
                        $"address varchar(100)," +
                        $"email varchar(100)," +
                        $"phone_number varchar(100)," +
                        $"date_of_birth varchar(100)); " +
                        $"create table if not exists {CONTRACTS_TABLE} (legal_person varchar (100), natural_person varchar (100), summ_contract int, status varchar (100), date_of_singning timestamp);", _npgsqlConnection);
                });
                if (!resCreateBaseTables.IsOk)
                    throw new Exception($"Произошли ошибка с инициализацией команды поиска схемы: {resCreateBaseTables.ErrorMessage}");
                var resCreateBaseTablesReader = new Result<NpgsqlDataReader>(() =>
                {
                    return resCreateBaseTables.ResultObject.ExecuteReader();
                });
                if (!resCreateBaseTablesReader.IsOk)
                    throw new Exception($"При чтении результата добваления таблиц, произошла ошибка {resCreateBaseTablesReader.ErrorMessage}");

                _npgsqlConnection.Close();
                _npgsqlConnection.Open();

                return true;
            });
        }

        public Result<int> GetSumInOneYear() 
        {
            return new Result<int>(() =>
            {

                var resCommandSumFromDB = new Result<NpgsqlCommand>(() =>
                {
                    return new NpgsqlCommand($"select SUM(summ_contract) from {CONTRACTS_TABLE} WHERE date_part('year', date_of_singning) = date_part('year', CURRENT_DATE) and status = 'complite'", _npgsqlConnection);
                });

                var resReadCommandSumFromDB = new Result<NpgsqlDataReader>(() =>
                {
                    return resCommandSumFromDB.ResultObject.ExecuteReader();
                });

                int sum = 0;
                while (resReadCommandSumFromDB.ResultObject.Read()) 
                {
                    sum = resReadCommandSumFromDB.ResultObject.GetFieldValue<int>(0);
                }

                _npgsqlConnection.Close();
                _npgsqlConnection.Open();

                return sum;
            });
        }

        public Result<int> GetSumFromRus()
        {
            return new Result<int>(() =>
            {

                var resCommandSumFromDB = new Result<NpgsqlCommand>(() =>
                {
                    return new NpgsqlCommand($"select SUM(summ_contract) from {CONTRACTS_TABLE} where legal_person in (select name_company from {LEGAL_PERSONS_TABLE} where contry = 'Russia') and status = 'complite'", _npgsqlConnection);
                });

                var resReadCommandSumFromDB = new Result<NpgsqlDataReader>(() =>
                {
                    return resCommandSumFromDB.ResultObject.ExecuteReader();
                });

                int sum = 0;
                while (resReadCommandSumFromDB.ResultObject.Read())
                {
                    sum = resReadCommandSumFromDB.ResultObject.GetFieldValue<int>(0);
                }

                _npgsqlConnection.Close();
                _npgsqlConnection.Open();

                return sum;
            });
        }

        public Result<string> GetListEmail()
        {
            return new Result<string>(() =>
            {
                string emails = string.Empty;
                var resCommandList = new Result<NpgsqlCommand>(() =>
                {
                    return new NpgsqlCommand($"select email from {NATURAL_PERSONS_TABLE} where place_of_work in (select name_company from {LEGAL_PERSONS_TABLE} where name_company in " +
                        $"(select legal_person from {CONTRACTS_TABLE} where summ_contract > 40000 and date_of_singning > current_timestamp - interval '30 day'))", _npgsqlConnection);
                });

                var resReadCommandList = new Result<NpgsqlDataReader>(() =>
                {
                    return resCommandList.ResultObject.ExecuteReader();
                });

                while (resReadCommandList.ResultObject.Read())
                {
                    var resReadEmail = resReadCommandList.ResultObject.GetFieldValue<string>(0);
                    emails += resReadEmail + "\n";
                }

                _npgsqlConnection.Close();
                _npgsqlConnection.Open();

                return emails;
            });
        }

        public Result<bool> ChangeStatus() 
        {
            return new Result<bool>(() =>
            {
                var resCommandChangeStatus = new Result<NpgsqlCommand>(() =>
                {
                    return new NpgsqlCommand($"update {CONTRACTS_TABLE} set status = 'terminate' where legal_person in (select name_company from {LEGAL_PERSONS_TABLE} " +
                        $"where name_company in (select place_of_work from {NATURAL_PERSONS_TABLE} where age >= 60))", _npgsqlConnection);

                });
                var resChangeStatus = new Result<bool>(() =>
                {
                    resCommandChangeStatus.ResultObject.ExecuteNonQuery();
                    return true;
                });
                if (!resChangeStatus.IsOk)
                    throw new Exception($"Не удалось внести изменения в таблице: {resChangeStatus.ErrorMessage}");


                _npgsqlConnection.Close();
                _npgsqlConnection.Open();

                return true;
            });
        }

        public Result<List<string>> UploadingDataForReport()
        {
            return new Result<List<string>>(() =>
            {
                List<string> report = new List<string>();

                var resCommandReport = new Result<NpgsqlCommand>(() =>
                {
                    return new NpgsqlCommand($"select (name, surname, patronymic, email, phone_number, date_of_birth) from {NATURAL_PERSONS_TABLE} where place_of_work in (select name_company from {LEGAL_PERSONS_TABLE} where name_company in " +
                        $"(select legal_person from {CONTRACTS_TABLE} where status = 'concluded') and city = 'Moscow')", _npgsqlConnection);

                });
                var resReadReport = new Result<NpgsqlDataReader>(() =>
                {
                    return resCommandReport.ResultObject.ExecuteReader();
                });
                if (!resReadReport.IsOk)
                    throw new Exception($"Не удалось считать данные в таблице: {resReadReport.ErrorMessage}");
                while (resReadReport.ResultObject.Read()) 
                {
                    string rep = string.Empty;
                    var resReport = resReadReport.ResultObject.GetFieldValue<object>(0) as Array;
                    foreach (var obj in resReport) 
                    {
                        rep += obj.ToString() + " | ";
                    }
                    rep += ";";
                    report.Add(rep);
                }
                ;
                _npgsqlConnection.Close();
                _npgsqlConnection.Open();

                return report;
            });
        }

        #endregion
    }
}
