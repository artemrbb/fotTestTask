using crm.Program;
using UltimateCore.AppManagement;

namespace crm
{
    class App
    {
        private static AppFactory _appFactory;
        private static ProgramWorker _programWorker; 

        static void Main(string[] args)
        {
            _appFactory = AppFactory.GetInstance();

            _programWorker = _appFactory.GetClass<ProgramWorker>();
            _programWorker.Start();
        }
    }
}
