﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateCore.AppManagement;
using UltimateCore.LRI;

namespace crm.Program
{
    sealed class ProgramWorker : Singleton<ProgramWorker>
    {
        #region Contructor

        public ProgramWorker()
        {
            _appFactory = AppFactory.GetInstance();
            _programControls = _appFactory.GetClass<ProgramControls>();

        }

        #endregion

        #region Fields

        private readonly AppFactory _appFactory;
        private readonly ProgramControls _programControls;


        #endregion

        #region Methods

        public Result<bool> Start()
        {
            return new Result<bool>(() =>
            {
                _programControls.Initialize();

                ViewConsole("Добро пожаловать, воспользуйтесь командой /help");

                while (true)
                    ReadConsole();



                return true;
            });
        }

        private Result<bool> ViewConsole(string informationForClient)
        {
            return new Result<bool>(() =>
            {
                Console.WriteLine(informationForClient);
                return true;
            });
        }

        private Result<bool> ReadConsole()
        {
            return new Result<bool>(() =>
            {
                var resInfo = _programControls.GetInfofromDB(Console.ReadLine());
                if (!resInfo.IsOk)
                    ViewConsole("Произошла ошибка!");

                ViewConsole(resInfo.ResultObject);
                return true;
            });
        }

        #endregion
    }
}
