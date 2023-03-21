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
                _npgsqlConnection = new NpgsqlConnection($"Host={host};Username={username};Password={password};Database={database}");
                _npgsqlConnection.Open();



                return true;
            });
        }

        #endregion
    }
}
