using crm.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateCore.AppManagement;
using UltimateCore.LRI;

namespace crm.Program
{
    sealed class ProgramControls : Singleton<ProgramControls>
    {
        #region Constructor

        public ProgramControls()
        {
            _appFactory = AppFactory.GetInstance();
            _dBMaster = _appFactory.GetClass<DBMaster>();
        }

        #endregion

        #region Fields

        private readonly AppFactory _appFactory;
        private readonly DBMaster _dBMaster;

        #endregion

        #region Methods

        public Result<bool> Initialize()
        {
            return new Result<bool>(() =>
            {
                var resConnectionDB = _dBMaster.Connection("localhost", "postgres", "linkoln", "fortest");
                if (!resConnectionDB.IsOk)
                    throw new Exception($"При подключении к БД, произошла ошибка: {resConnectionDB}");


                return true;
            });
        }

        public Result<string> GetInfofromDB(string command)
        {
            return new Result<string>(() =>
            {

                if (string.IsNullOrEmpty(command))
                    return "Команда пуста. Воспользуйтесь командой /help";
                else if (command == "/inoneyear") // 1.	Вывести сумму всех заключенных договоров за текущий год.
                {
                    var result = _dBMaster.GetSumInOneYear();
                    if (!result.IsOk)
                        throw new Exception($"При запросе, произошла ошибка {result.ErrorMessage}");

                    return result.ResultObject.ToString();
                }
                else if (command == "/fromrus") // 2.	Вывести сумму заключенных договоров по каждому контрагенту из России.
                {
                    var result = _dBMaster.GetSumFromRus();
                    if (!result.IsOk)
                        throw new Exception($"При запросе, произошла ошибка {result.ErrorMessage}");

                    return result.ResultObject.ToString();
                }
                else if (command == "/emails")// вывести email физ лиц заключивших договор за последние 30 дней на сумму больше 40000 
                {
                    var result = _dBMaster.GetListEmail();
                    if (!result.IsOk)
                        throw new Exception($"При запросе, произошла ошибка {result.ErrorMessage}");

                    return result.ResultObject.ToString();
                }
                else if (command == "/changestatus") // 4.	Изменить статус договора на "Расторгнут" для физических лиц, у которых есть действующий договор, и возраст которых старше 60 лет включительно.
                {
                    var result = _dBMaster.ChangeStatus();
                    if (!result.IsOk)
                        throw new Exception($"При запросе, произошла ошибка {result.ErrorMessage}");

                    return "Изменения внесены!";
                }
                else if (command == "/createreport") // 5.	Создать отчет (текстовый файл, например, в формате xml, xlsx, json) содержащий ФИО, e-mail, моб. телефон, дату рождения физ. лиц, у которых есть действующие договора по компаниям, расположенных в городе Москва. 
                {
                    var result = _dBMaster.UploadingDataForReport();
                    if (!result.IsOk)
                        throw new Exception($"При запросе, произошла ошибка {result.ErrorMessage}");

                    using (StreamWriter sw = new StreamWriter("C:\\Users\\pc\\Downloads\\Report.txt", true, Encoding.ASCII)) 
                    {
                        foreach (var str in result.ResultObject) 
                        {
                            sw.WriteLine(str);
                        }
                    }
                }
                else if (command == "/help")
                {
                    return $"/inoneyear - Вывести сумму всех заключенных договоров за текущий год\n" +
                    $"/fromrus - Вывести сумму заключенных договоров по каждому контрагенту из России\n" +
                    $"/emails - Вывести email физ лиц заключивших договор за последние 30 дней на сумму больше 40000\n" +
                    $"/changestatus - Изменить статус договора на \"Расторгнут\" для физических лиц, у которых есть действующий договор, и возраст которых старше 60 лет включительно.\n" +
                    $"/createreport - Создать отчет (текстовый файл, например, в формате xml, xlsx, json) содержащий ФИО, e-mail, моб. телефон, дату рождения физ. лиц, у которых есть действующие договора по компаниям, расположенных в городе Москва";
                }
                else
                    return "Команда не найдена";

                return null;
            });
        }

        #endregion
    }
}
