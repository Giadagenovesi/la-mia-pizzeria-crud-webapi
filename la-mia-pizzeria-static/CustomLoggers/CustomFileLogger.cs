namespace la_mia_pizzeria_static.CustomLoggers
{
    public class CustomFileLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            File.AppendAllText("C:\\Users\\giada\\source\\repos\\la-mia-pizzeria-static\\la-mia-pizzeria-static\\my-log.txt", $"{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} LOG: {message}\n");
        }
    }
}
