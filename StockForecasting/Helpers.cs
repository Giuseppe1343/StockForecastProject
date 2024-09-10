using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockForecasting
{
    internal class Helpers
    {
        private readonly static Dictionary<LogLevel,(ConsoleColor Color, MessageBoxIcon Icon)> Args = new()
        {
            { LogLevel.Info, (ConsoleColor.Blue, MessageBoxIcon.Information) },
            { LogLevel.Error, (ConsoleColor.Red, MessageBoxIcon.Error) },
            { LogLevel.Warning, (ConsoleColor.DarkYellow, MessageBoxIcon.Warning) }
        };
        private static void WriteOutput(string message, LogLevel type)
        {
            switch (Channel)
            {
                case OutputChannel.Console:
                    Console.ForegroundColor = Args[type].Color;
                    Console.Write(type.ToString() + ": ");
                    Console.ResetColor();
                    Console.WriteLine(message);
                    break;
                case OutputChannel.MessageBox:
                    MessageBox.Show(message, type.ToString(), MessageBoxButtons.OK, Args[type].Icon);
                    break;
                case OutputChannel.Custom:
                    Debug.Assert(false, "Not implemented yet");
                    break;
                default:
                    Debug.Assert(false, "Invalid Output Channel");
                    break;
            }
        }

        public static OutputChannel Channel { get; set; } = OutputChannel.Console;
        public static void InfoOutput(string message) => WriteOutput(message, LogLevel.Info);
        public static void ErrorOutput(string message) => WriteOutput(message, LogLevel.Error);
        public static void WarningOutput(string message) => WriteOutput(message, LogLevel.Warning);

        public static string GetConnectionString()
        {
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "config.txt";
            if (!File.Exists(configPath))
                File.WriteAllText(configPath, "Server= ;Database= ;User Id= ;Password= ;Trust Server Certificate=True;");
            return File.ReadAllText(configPath); ;
        }
    }

}
