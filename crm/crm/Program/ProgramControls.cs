using System;
using System.Collections.Generic;
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
        }

        #endregion

        #region Fields

        private readonly AppFactory _appFactory;

        #endregion

        #region Methods

        public Result<bool> Initialize()
        {
            return new Result<bool>(() =>
            {

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

                }
                else if (command == "/fromrus") // 2.	Вывести сумму заключенных договоров по каждому контрагенту из России.
                {

                }
                else if (command == "/emails")// вывести email физ лиц заключивших договор за последние 30 дней на сумму больше 40000 
                {

                }
                else if (command == "/changestatus") // 4.	Изменить статус договора на "Расторгнут" для физических лиц, у которых есть действующий договор, и возраст которых старше 60 лет включительно.
                {

                }
                else if (command == "/createreport") // 5.	Создать отчет (текстовый файл, например, в формате xml, xlsx, json) содержащий ФИО, e-mail, моб. телефон, дату рождения физ. лиц, у которых есть действующие договора по компаниям, расположенных в городе Москва. 
                {

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
