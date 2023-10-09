using System.Diagnostics;

namespace la_mia_pizzeria_static.CustomLoggers
{
    public class CustomConsoleLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            Debug.WriteLine("LOG: " + message);
        }
    }
}
