using System.Collections.Concurrent;
using System.Diagnostics;

namespace StockForecasting
{
    public class Helpers
    {
        public static string GetConnectionString()
        {
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "config.txt";
            if (!File.Exists(configPath))
                File.WriteAllText(configPath, "Server= ;Database= ;User Id= ;Password= ;Trust Server Certificate=True;");
            return File.ReadAllText(configPath); ;
        }
        public static class MessageOutput
        {
            public static event EventHandler<MessageReceivedEventArgs> MessageReceived;

            private static void WriteOutput(string message, LogLevel type)
            {
                MessageReceived?.Invoke(null, new MessageReceivedEventArgs(message, type));
            }
            public static void InfoOutput(string message) => WriteOutput(message, LogLevel.Info);
            public static void ErrorOutput(string message) => WriteOutput(message, LogLevel.Error);
            public static void WarningOutput(string message) => WriteOutput(message, LogLevel.Warning);
        }
        public class MessageReceivedEventArgs : EventArgs
        {
            public string Message { get; set; }
            public LogLevel Type { get; set; }

            public MessageReceivedEventArgs(string message, LogLevel type)
            {
                Message = message;
                Type = type;
            }
        }
    }
}
