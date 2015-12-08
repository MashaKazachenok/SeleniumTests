using log4net;
using log4net.Config;

namespace Model
{
    public class Logger
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Logger));

        public static ILog Log
        {
            get { return _log; }
        }

        public static void Initialize()
        {
            XmlConfigurator.Configure();
        }
    }
}
