using crm.DB;
using crm.Models;
using Microsoft.Office.Interop.Excel;
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
                var resConnectionDB = _dBMaster.Connection();
                if (!resConnectionDB.IsOk)
                    throw new Exception($"При подключении к БД, произошла ошибка: {resConnectionDB.ErrorMessage}");


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

                    var resCreateExcel = CreateExcel(result.ResultObject);
                    if (!resCreateExcel.IsOk)
                        throw new Exception($"При записи данных в Excel, произошла ошибка");

                    return "Отчет создан!";
                }
                else if (command == "/help")
                {
                    return $"\n/inoneyear - Вывести сумму всех заключенных договоров за текущий год\n" +
                    $"/fromrus - Вывести сумму заключенных договоров по каждому контрагенту из России\n" +
                    $"/emails - Вывести email физ лиц заключивших договор за последние 30 дней на сумму больше 40000\n" +
                    $"/changestatus - Изменить статус договора на \"Расторгнут\" для физических лиц, у которых есть действующий договор, и возраст которых старше 60 лет включительно.\n" +
                    $"/createreport - Создать отчет (текстовый файл, например, в формате xml, xlsx, json) содержащий ФИО, e-mail, моб. телефон, дату рождения физ. лиц, у которых есть действующие договора по компаниям, расположенных в городе Москва\n\n" +
                    $"Отчёт создается в Debug програмы\n";
                }
                else
                    return "Команда не найдена";

                return null;
            });
        }

        private Result<bool> CreateExcel(List<NaturalPersonModel> report) 
        {
            return new Result<bool>(() =>
            {
                using (ExcelHelper helper = new ExcelHelper()) 
                {
                    var resOpen = helper.Open(Path.Combine(Environment.CurrentDirectory, "Report.xls"));
                    if (!resOpen.IsOk)
                        throw new Exception("При инициализации файла выборки, произошла ошибка");


                    for (var i = 0; i < report.Count; i++) 
                    {
                        helper.Set("A", i + 1, report[i].Name);
                        helper.Set("B", i + 1, report[i].Surname);
                        helper.Set("C", i + 1, report[i].Patronymic);
                        helper.Set("D", i + 1, report[i].Email);
                        helper.Set("E", i + 1, report[i].PhoneNumber);
                        helper.Set("F", i + 1, report[i].DateOfBirth);
                    }
                    // helper.Set(); тут внести в таблицу

                    helper.Save();
                }

                return true;
            });
        }

        #endregion
    }
}
