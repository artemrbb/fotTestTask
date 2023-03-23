using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateCore.LRI;
using Excel = Microsoft.Office.Interop.Excel;

namespace crm.Program
{
    class ExcelHelper : IDisposable
    {
        #region Constructor

        public ExcelHelper()
        {
            _excel = new Excel.Application();
        }

        #endregion

        #region Fields

        private Application _excel;
        private Workbook _workBook;
        private string _filePath;

        #endregion

        #region Methods

        public Result<bool> Open(string filePath) 
        {
            return new Result<bool>(() =>
            {
                if (File.Exists(filePath))
                {
                    _workBook = _excel.Workbooks.Open(filePath);
                }
                else 
                {
                    _workBook = _excel.Workbooks.Add();
                    _filePath = filePath;
                }

                return true;
            });
        }

        public void Dispose()
        {
            var resultCloce = new Result<bool>(() =>
            {
                _workBook.Close();
                return true;
            });
            if (!resultCloce.IsOk) 
            {
                // сюда вписать логгер
            }
        }

        public Result<bool> Set(string column, int row, string date) 
        {
            return new Result<bool>(() =>
            {
                ((Excel.Worksheet)_excel.ActiveSheet).Cells[row, column] = date;

                return true;
            });
        }

        public Result<bool> Save() 
        {
            return new Result<bool>(() =>
            {
                if (!string.IsNullOrEmpty(_filePath))
                {
                    _workBook.SaveAs(_filePath);
                    _filePath = null;
                }
                else 
                {
                    _workBook.Save();
                }


                return true;
            });
        }

        #endregion


    }
}
